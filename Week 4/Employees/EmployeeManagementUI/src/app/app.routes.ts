import { Routes } from '@angular/router';
import { EmployeeList } from './features/employees/employee-list/employee-list';
import { EmployeeAdd } from './features/employees/employee-add/employee-add';
import { Register } from './features/auth/register/register';
import { Login } from './features/auth/login/login';
import { authGuard } from './core/guards/auth-guard';
import { EmployeeDetails } from './features/employees/employee-details/employee-details';

export const routes: Routes = [
 
  {path: 'employees/:id' , component: EmployeeDetails},

  { path: 'login', component: Login },
  { path: 'register', component: Register },

  {
    path: 'employees', component: EmployeeList, canActivate: [authGuard]
  },
  {
    path: 'employees/add',  component: EmployeeAdd, canActivate: [authGuard]
  },
  { path: '**', redirectTo: 'login', pathMatch: 'full' }

];