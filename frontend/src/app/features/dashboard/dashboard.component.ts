import { Component, OnInit, signal, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { TransactionApiService, TransactionDto } from '../../core/services/transaction-api.service';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink, MatCardModule, MatButtonModule, MatIconModule],
  templateUrl: './dashboard.component.html'
})
export class DashboardComponent implements OnInit {
  private txApi = inject(TransactionApiService);
  readonly auth = inject(AuthService);

  readonly transactions = signal<TransactionDto[]>([]);
  readonly loading = signal(true);

  readonly totalIncome = computed(() =>
    this.transactions().filter(t => t.type === 'Income').reduce((s, t) => s + t.amount, 0)
  );
  readonly totalExpenses = computed(() =>
    this.transactions().filter(t => t.type === 'Expense').reduce((s, t) => s + t.amount, 0)
  );
  readonly balance = computed(() => this.totalIncome() - this.totalExpenses());
  readonly recentTransactions = computed(() => this.transactions().slice(0, 5));

  readonly currentMonth = new Date().toLocaleString('default', { month: 'long', year: 'numeric' });

  ngOnInit() {
    const now = new Date();
    this.txApi.getAll(now.getMonth() + 1, now.getFullYear()).subscribe({
      next: txs => { this.transactions.set(txs); this.loading.set(false); },
      error: () => this.loading.set(false)
    });
  }
}
