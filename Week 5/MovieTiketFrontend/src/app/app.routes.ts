import { Routes } from '@angular/router';
import { AdminLogin } from './pages/admin-login/admin-login';
import { AdminDashboard } from './pages/admin-dashboard/admin-dashboard';
import { authGuard } from './guards/auth-guard';

export const routes: Routes = [

 {
        path: 'admin/dashboard',
        component: AdminDashboard,
        canActivate: [authGuard]
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