import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_CONFIG } from '../config/api.config';
import { Observable } from 'rxjs';
import { RegisterRequest } from '../../models/register-request';
import { LoginRequest } from '../../models/login-request';
import { RefreshTokenRequest } from '../../models/RefreshTokenRequest ';

@Injectable({
  providedIn: 'root',
})
export class Auth {
  private http = inject(HttpClient);

  private apiUrl= `${API_CONFIG.baseUrl}/auth`

  register(data:RegisterRequest) : Observable<any>{
    return this.http.post<any>(`${this.apiUrl}/Register`,data);
  }

  login(data:LoginRequest) : Observable<any>{
    return this.http.post<any>(`${this.apiUrl}/login`,data);
  }

  refreshToken(data:RefreshTokenRequest) : Observable <any>{
    return this.http.post<any>(`${this.apiUrl}/refresh`,data);
  }

}
