import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-admin-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './admin-login.html',
  styleUrl: './admin-login.css'
})
export class AdminLogin {
  private fb = inject(FormBuilder).nonNullable;
  private authService = inject(AuthService);
  private router = inject(Router);

  activeTab = signal<'login' | 'register'>('login');
  loading = signal(false);
  errorMessage = signal('');
  successMessage = signal('');

  loginForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    passWord: ['', Validators.required]
  });

  registerForm = this.fb.group({
    userName: ['', [Validators.required, Validators.minLength(3)]],
    email: ['', [Validators.required, Validators.email]],
    passWord: ['', [Validators.required, Validators.minLength(6)]]
  });

  switchTab(tab: 'login' | 'register'): void {
    this.activeTab.set(tab);
    this.errorMessage.set('');
    this.successMessage.set('');
  }

  onLogin(): void {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.loading.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');

    this.authService.loginAdmin(this.loginForm.getRawValue()).subscribe({
      next: () => {
        this.loading.set(false);
        this.router.navigate(['/admin/dashboard']);
      },
      error: (error) => {
        this.errorMessage.set(
          error.error?.message || 'Admin authentication failed. Please check your credentials.'
        );
        this.loading.set(false);
      }
    });
  }

  onRegister(): void {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    this.loading.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');

    this.authService.registerAdmin(this.registerForm.getRawValue()).subscribe({
      next: () => {
        this.loading.set(false);
        this.successMessage.set('Admin account created! Please sign in.');
        this.switchTab('login');
      },
      error: (error) => {
        this.errorMessage.set(
          error.error?.message || 'Admin registration failed.'
        );
        this.loading.set(false);
      }
    });
  }
}
