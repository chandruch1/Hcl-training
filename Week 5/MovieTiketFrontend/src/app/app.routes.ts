import { Routes } from '@angular/router';
import { AdminLogin } from './pages/admin-login/admin-login';

export const routes: Routes = [
  {
    path: 'admin/login',
    component: AdminLogin
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