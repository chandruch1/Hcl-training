import { Routes } from '@angular/router';
import { AdminLogin } from './pages/admin-login/admin-login';
import { AdminDashboard } from './pages/admin-dashboard/admin-dashboard';

export const routes: Routes = [
  {
    path: 'admin/login',
    component: AdminLogin
  },
  {
    path: 'admin/dashboard',
    component: AdminDashboard
  },
  {
    path: '',
    redirectTo: 'admin/login',
    pathMatch: 'full'
  },
  {
    path: '**',
    redirectTo: 'admin/login'
  }
];