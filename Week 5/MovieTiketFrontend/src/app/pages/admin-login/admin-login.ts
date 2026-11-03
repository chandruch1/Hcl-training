import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { AdminAuth } from '../../services/admin-auth';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-login',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './admin-login.html',
  styleUrl: './admin-login.css'
})
export class AdminLogin {

  private fb = inject(FormBuilder).nonNullable;
  private adminAuth = inject(AdminAuth);
  private router = inject(Router);

  loading = signal(false);
  errorMessage = signal('');
  successMessage = signal('');

  loginForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    passWord: ['', Validators.required]
  });

  onLogin() {

    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.loading.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');

    this.adminAuth.login(this.loginForm.getRawValue()).subscribe({
      next: (response) => {
        sessionStorage.setItem('accessToken', response.accessToken);
        sessionStorage.setItem('adminName', response.name);
        sessionStorage.setItem('adminRole', response.role);

        this.loading.set(false);
        this.router.navigate(['/admin/dashboard']);
      },
      error: (error) => {
        this.errorMessage.set(
          error.error?.message || 'Login failed. Please try again.'
        );
        this.loading.set(false);
      }
    });
  }
}