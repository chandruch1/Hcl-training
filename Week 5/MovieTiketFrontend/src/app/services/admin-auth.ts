import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_BASE_URL } from '../config/api.config';
import { AdminLoginRequest } from '../models/admin-login-request';
import { AdminLoginResponse } from '../models/admin-login-response';

@Injectable({
  providedIn: 'root'
})
export class AdminAuth {

  private http = inject(HttpClient);

  private apiUrl = `${API_BASE_URL}/api/Admin`;

  login(data: AdminLoginRequest) {
    return this.http.post<AdminLoginResponse>(
      `${this.apiUrl}/login`,
      data
    );
  }
}