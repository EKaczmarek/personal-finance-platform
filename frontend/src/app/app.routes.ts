import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  {
    path: 'login',
    loadComponent: () => import('./features/auth/login/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'register',
    loadComponent: () => import('./features/auth/register/register.component').then(m => m.RegisterComponent)
  },
  {
    path: 'dashboard',
    canActivate: [authGuard],
    loadComponent: () => import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent)
  },
  {
    path: 'transactions',
    canActivate: [authGuard],
    loadComponent: () => import('./features/transactions/transactions.component').then(m => m.TransactionsComponent)
  },
  {
    path: 'budgets',
    canActivate: [authGuard],
    loadComponent: () => import('./features/budgets/budgets.component').then(m => m.BudgetsComponent)
  },
  { path: '**', redirectTo: 'dashboard' }
];
