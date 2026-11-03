import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../config/api.config';
import {
  Theatre,
  CreateTheatreDto,
  UpdateTheatreDto
} from '../models/theatre.model';

@Injectable({
  providedIn: 'root'
})
export class TheatreService {
  private http = inject(HttpClient);
  private apiUrl = `${API_BASE_URL}/api/Theatres`;

  getAll(): Observable<Theatre[]> {
    return this.http.get<Theatre[]>(this.apiUrl);
  }

  getById(id: number): Observable<Theatre> {
    return this.http.get<Theatre>(`${this.apiUrl}/${id}`);
  }

  create(dto: CreateTheatreDto): Observable<Theatre> {
    return this.http.post<Theatre>(this.apiUrl, dto);
  }

  update(id: number, dto: UpdateTheatreDto): Observable<Theatre> {
    return this.http.put<Theatre>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}

