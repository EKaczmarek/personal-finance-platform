import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  email: string;
  firstName: string;
  lastName: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly _accessToken = signal<string | null>(null);
  private readonly _user = signal<{ email: string; firstName: string; lastName: string } | null>(null);

  readonly isLoggedIn = computed(() => this._accessToken() !== null);
  readonly currentUser = computed(() => this._user());

  constructor(private http: HttpClient, private router: Router) {
    const stored = sessionStorage.getItem('refresh_token');
    if (stored) this._tryRestoreSession(stored);
  }

  login(email: string, password: string) {
    return this.http.post<AuthResponse>(`${environment.apiUrl}/auth/login`, { email, password }).pipe(
      tap(res => this._applySession(res))
    );
  }

  register(email: string, password: string, firstName: string, lastName: string) {
    return this.http.post<AuthResponse>(`${environment.apiUrl}/auth/register`, { email, password, firstName, lastName }).pipe(
      tap(res => this._applySession(res))
    );
  }

  refresh() {
    const token = sessionStorage.getItem('refresh_token');
    if (!token) return;
    return this.http.post<AuthResponse>(`${environment.apiUrl}/auth/refresh`, { token }).pipe(
      tap(res => this._applySession(res))
    );
  }

  logout() {
    const token = sessionStorage.getItem('refresh_token');
    if (token) {
      this.http.post(`${environment.apiUrl}/auth/logout`, { refreshToken: token }).subscribe();
    }
    this._clearSession();
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    return this._accessToken();
  }

  private _applySession(res: AuthResponse) {
    this._accessToken.set(res.accessToken);
    this._user.set({ email: res.email, firstName: res.firstName, lastName: res.lastName });
    sessionStorage.setItem('refresh_token', res.refreshToken);
  }

  private _clearSession() {
    this._accessToken.set(null);
    this._user.set(null);
    sessionStorage.removeItem('refresh_token');
  }

  private _tryRestoreSession(refreshToken: string) {
    this.http.post<AuthResponse>(`${environment.apiUrl}/auth/refresh`, { token: refreshToken }).subscribe({
      next: res => this._applySession(res),
      error: () => this._clearSession()
    });
  }
}
