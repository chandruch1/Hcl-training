import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { API_BASE_URL } from '../config/api.config';
import {
  LoginRequest,
  RegisterRequest,
  AdminRegisterRequest,
  AuthResponse,
  CurrentUser
} from '../models/auth.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);

  private readonly TOKEN_KEY = 'accessToken';
  private readonly USER_KEY = 'currentUser';

  currentUser = signal<CurrentUser | null>(this.loadUserFromStorage());
  isLoggedIn = computed(() => !!this.currentUser());
  isAdmin = computed(() => {
    const role = this.currentUser()?.role?.toString().toLowerCase();
    return role === 'admin' || role === '1';
  });

  private loadUserFromStorage(): CurrentUser | null {
    try {
      const stored = sessionStorage.getItem(this.USER_KEY);
      if (stored) {
        return JSON.parse(stored);
      }
      const token = sessionStorage.getItem(this.TOKEN_KEY);
      if (token) {
        const adminName = sessionStorage.getItem('adminName') || 'Admin';
        const role = sessionStorage.getItem('adminRole') || 'Admin';
        return {
          token,
          name: adminName,
          email: '',
          role: role
        };
      }
    } catch {
      // ignore parsing error
    }
    return null;
  }

  loginCustomer(data: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${API_BASE_URL}/api/Auth/login`, data).pipe(
      tap((res) => {
        const token = res.accessToken || res.accesToken || '';
        const role = res.Role?.toString() === '3' ? 'Customer' : (res.role?.toString() || 'Customer');
        const user: CurrentUser = {
          token,
          name: res.name || (res.Username ? `User #${res.Username}` : 'Customer'),
          email: data.email,
          role
        };
        this.saveSession(user);
      })
    );
  }

  registerCustomer(data: RegisterRequest): Observable<any> {
    return this.http.post<any>(`${API_BASE_URL}/api/Auth/register`, data);
  }

  loginAdmin(data: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${API_BASE_URL}/api/Admin/login`, data).pipe(
      tap((res) => {
        const token = res.accessToken || res.accesToken || '';
        const role = res.role?.toString() || 'Admin';
        const user: CurrentUser = {
          token,
          name: res.name || 'Admin',
          email: res.email || data.email,
          role
        };
        this.saveSession(user);
      })
    );
  }

  registerAdmin(data: AdminRegisterRequest): Observable<any> {
    return this.http.post<any>(`${API_BASE_URL}/api/Admin/Register`, data);
  }

  private saveSession(user: CurrentUser): void {
    sessionStorage.setItem(this.TOKEN_KEY, user.token);
    sessionStorage.setItem(this.USER_KEY, JSON.stringify(user));
    sessionStorage.setItem('adminName', user.name);
    sessionStorage.setItem('adminRole', user.role);
    this.currentUser.set(user);
  }

  logout(): void {
    sessionStorage.removeItem(this.TOKEN_KEY);
    sessionStorage.removeItem(this.USER_KEY);
    sessionStorage.removeItem('adminName');
    sessionStorage.removeItem('adminRole');
    this.currentUser.set(null);
  }

  getToken(): string | null {
    return this.currentUser()?.token || sessionStorage.getItem(this.TOKEN_KEY);
  }
}

