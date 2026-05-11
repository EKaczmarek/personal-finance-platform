import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../../environments/environment';

export interface TransactionDto {
  id: string;
  amount: number;
  description: string;
  type: 'Income' | 'Expense';
  occurredAt: string;
  accountId: string;
  accountName: string;
  categoryId: string;
  categoryName: string;
  categoryColor: string;
  createdAt: string;
}

export interface CreateTransactionRequest {
  accountId: string;
  categoryId: string;
  amount: number;
  type: 'Income' | 'Expense';
  description: string;
  occurredAt?: string;
}

@Injectable({ providedIn: 'root' })
export class TransactionApiService {
  private readonly base = `${environment.apiUrl}/transactions`;

  constructor(private http: HttpClient) {}

  getAll(month?: number, year?: number) {
    let params = new HttpParams();
    if (month) params = params.set('month', month);
    if (year) params = params.set('year', year);
    return this.http.get<TransactionDto[]>(this.base, { params });
  }

  create(request: CreateTransactionRequest) {
    return this.http.post<TransactionDto>(this.base, request);
  }

  delete(id: string) {
    return this.http.delete(`${this.base}/${id}`);
  }
}
