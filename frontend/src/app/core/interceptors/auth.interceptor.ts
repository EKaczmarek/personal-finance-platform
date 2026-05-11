import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(AuthService);
  const router = inject(Router);

  const token = auth.getToken();
  const authReq = token
    ? req.clone({ setHeaders: { Authorization: `Bearer ${token}` } })
    : req;

  return next(authReq).pipe(
    catchError((err: HttpErrorResponse) => {
      if (err.status === 401 && !req.url.includes('/auth/')) {
        const refresh$ = auth.refresh();
        if (refresh$) {
          return refresh$.pipe(
            switchMap(() => {
              const retryToken = auth.getToken();
              const retried = req.clone({ setHeaders: { Authorization: `Bearer ${retryToken}` } });
              return next(retried);
            }),
            catchError(() => {
              auth.logout();
              return throwError(() => err);
            })
          );
        }
        auth.logout();
        router.navigate(['/login']);
      }
      return throwError(() => err);
    })
  );
};
