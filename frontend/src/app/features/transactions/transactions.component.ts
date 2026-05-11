import { Component, OnInit, signal, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { TransactionApiService, TransactionDto } from '../../core/services/transaction-api.service';

@Component({
  selector: 'app-transactions',
  standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule, RouterLink,
    MatCardModule, MatButtonModule, MatFormFieldModule,
    MatInputModule, MatSelectModule, MatIconModule, MatDividerModule
  ],
  templateUrl: './transactions.component.html'
})
export class TransactionsComponent implements OnInit {
  private txApi = inject(TransactionApiService);
  private fb = inject(FormBuilder);

  readonly transactions = signal<TransactionDto[]>([]);
  readonly loading = signal(true);
  readonly showForm = signal(false);
  readonly submitting = signal(false);

  readonly selectedMonth = signal(new Date().getMonth() + 1);
  readonly selectedYear = signal(new Date().getFullYear());

  readonly filtered = computed(() => this.transactions());

  readonly months = [
    { value: 1, label: 'January' }, { value: 2, label: 'February' },
    { value: 3, label: 'March' }, { value: 4, label: 'April' },
    { value: 5, label: 'May' }, { value: 6, label: 'June' },
    { value: 7, label: 'July' }, { value: 8, label: 'August' },
    { value: 9, label: 'September' }, { value: 10, label: 'October' },
    { value: 11, label: 'November' }, { value: 12, label: 'December' }
  ];

  addForm = this.fb.nonNullable.group({
    description: ['', Validators.required],
    amount: [0, [Validators.required, Validators.min(0.01)]],
    type: ['Expense' as 'Income' | 'Expense', Validators.required],
    accountId: ['', Validators.required],
    categoryId: ['', Validators.required]
  });

  ngOnInit() { this.load(); }

  load() {
    this.loading.set(true);
    this.txApi.getAll(this.selectedMonth(), this.selectedYear()).subscribe({
      next: txs => { this.transactions.set(txs); this.loading.set(false); },
      error: () => this.loading.set(false)
    });
  }

  onMonthChange(month: number) { this.selectedMonth.set(month); this.load(); }
  onYearChange(year: number) { this.selectedYear.set(year); this.load(); }

  submit() {
    if (this.addForm.invalid) return;
    this.submitting.set(true);
    const val = this.addForm.getRawValue();
    this.txApi.create({
      accountId: val.accountId,
      categoryId: val.categoryId,
      amount: val.amount,
      type: val.type,
      description: val.description
    }).subscribe({
      next: tx => {
        this.transactions.update(list => [tx, ...list]);
        this.addForm.reset({ type: 'Expense' });
        this.showForm.set(false);
        this.submitting.set(false);
      },
      error: () => this.submitting.set(false)
    });
  }

  delete(id: string) {
    this.txApi.delete(id).subscribe(() =>
      this.transactions.update(list => list.filter(t => t.id !== id))
    );
  }
}
