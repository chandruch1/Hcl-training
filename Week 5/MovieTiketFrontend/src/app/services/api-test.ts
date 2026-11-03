
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_BASE_URL } from '../config/api.config';

@Injectable({
  providedIn: 'root'
})
export class ApiTest {

  private http = inject(HttpClient);

  testConnection() {
    return this.http.get(`${API_BASE_URL}/api/Movies`);
  }
}
