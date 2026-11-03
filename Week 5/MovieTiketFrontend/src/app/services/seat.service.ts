import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../config/api.config';
import {
  Seat,
  CreateSeatDto,
  BulkCreateSeatDto
} from '../models/seat.model';

@Injectable({
  providedIn: 'root'
})
export class SeatService {
  private http = inject(HttpClient);
  private apiUrl = `${API_BASE_URL}/api/Seats`;

  getByScreen(screenId: number): Observable<Seat[]> {
    return this.http.get<Seat[]>(`${this.apiUrl}/screen/${screenId}`);
  }

  getById(id: number): Observable<Seat> {
    return this.http.get<Seat>(`${this.apiUrl}/${id}`);
  }

  create(dto: CreateSeatDto): Observable<Seat> {
    return this.http.post<Seat>(this.apiUrl, dto);
  }

  createBulk(dto: BulkCreateSeatDto): Observable<Seat[]> {
    return this.http.post<Seat[]>(`${this.apiUrl}/bulk`, dto);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}

