import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-budgets',
  standalone: true,
  imports: [CommonModule, RouterLink, MatCardModule, MatButtonModule],
  template: `
    <div class="page">
      <div class="page-header">
        <h1>Budgets</h1>
        <a mat-stroked-button routerLink="/dashboard">← Dashboard</a>
      </div>
      <mat-card>
        <mat-card-content>
          <p class="empty">Budget management coming in Week 3.</p>
        </mat-card-content>
      </mat-card>
    </div>
  `
})
export class BudgetsComponent {}
