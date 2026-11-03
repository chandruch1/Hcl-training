import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../config/api.config';
import {
  Screen,
  CreateScreenDto,
  UpdateScreenDto
} from '../models/screen.model';

@Injectable({
  providedIn: 'root'
})
export class ScreenService {
  private http = inject(HttpClient);
  private apiUrl = `${API_BASE_URL}/api/Screens`;

  getAll(): Observable<Screen[]> {
    return this.http.get<Screen[]>(this.apiUrl);
  }

  getById(id: number): Observable<Screen> {
    return this.http.get<Screen>(`${this.apiUrl}/${id}`);
  }

  getByTheatre(theatreId: number): Observable<Screen[]> {
    return this.http.get<Screen[]>(`${this.apiUrl}/theatre/${theatreId}`);
  }

  create(dto: CreateScreenDto): Observable<Screen> {
    return this.http.post<Screen>(this.apiUrl, dto);
  }

  update(id: number, dto: UpdateScreenDto): Observable<Screen> {
    return this.http.put<Screen>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}

