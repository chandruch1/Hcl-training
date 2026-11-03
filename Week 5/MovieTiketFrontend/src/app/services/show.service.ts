import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../config/api.config';
import {
  Show,
  CreateShowDto,
  UpdateShowDto,
  ShowSeat
} from '../models/show.model';

@Injectable({
  providedIn: 'root'
})
export class ShowService {
  private http = inject(HttpClient);
  private apiUrl = `${API_BASE_URL}/api/Shows`;

  getAll(): Observable<Show[]> {
    return this.http.get<Show[]>(this.apiUrl);
  }

  getById(id: number): Observable<Show> {
    return this.http.get<Show>(`${this.apiUrl}/${id}`);
  }

  getByMovie(movieId: number): Observable<Show[]> {
    return this.http.get<Show[]>(`${this.apiUrl}/movie/${movieId}`);
  }

  getByTheatre(theatreId: number): Observable<Show[]> {
    return this.http.get<Show[]>(`${this.apiUrl}/theatre/${theatreId}`);
  }

  getByDate(dateIso: string): Observable<Show[]> {
    return this.http.get<Show[]>(`${this.apiUrl}/date/${dateIso}`);
  }

  getSeats(showId: number): Observable<ShowSeat[]> {
    return this.http.get<ShowSeat[]>(`${this.apiUrl}/${showId}/seats`);
  }

  create(dto: CreateShowDto): Observable<Show> {
    return this.http.post<Show>(this.apiUrl, dto);
  }

  update(id: number, dto: UpdateShowDto): Observable<Show> {
    return this.http.put<Show>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}

