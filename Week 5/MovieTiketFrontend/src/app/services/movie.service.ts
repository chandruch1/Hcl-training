import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../config/api.config';
import {
  Movie,
  CreateMovieDto,
  UpdateMovieDto,
  MovieSummary,
  PagedMovieResponse
} from '../models/movie.model';

@Injectable({
  providedIn: 'root'
})
export class MovieService {
  private http = inject(HttpClient);
  private apiUrl = `${API_BASE_URL}/api/Movies`;

  getMovies(page = 1, pageSize = 12): Observable<PagedMovieResponse> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());
    return this.http.get<PagedMovieResponse>(this.apiUrl, { params });
  }

  getMovieById(id: number): Observable<Movie> {
    return this.http.get<Movie>(`${this.apiUrl}/${id}`);
  }

  searchMovies(title: string): Observable<MovieSummary[]> {
    const params = new HttpParams().set('title', title);
    return this.http.get<MovieSummary[]>(`${this.apiUrl}/search`, { params });
  }

  createMovie(dto: CreateMovieDto): Observable<Movie> {
    return this.http.post<Movie>(this.apiUrl, dto);
  }

  updateMovie(id: number, dto: UpdateMovieDto): Observable<Movie> {
    return this.http.put<Movie>(`${this.apiUrl}/${id}`, dto);
  }

  deleteMovie(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}

