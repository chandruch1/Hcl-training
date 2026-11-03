import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_BASE_URL } from '../config/api.config';
import { Movie } from '../models/movie';

export interface MovieListResponse {
  movies: Movie[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
}

@Injectable({
  providedIn: 'root'
})
export class ApiTest {

  private http = inject(HttpClient);

  getMovies() {
    return this.http.get<MovieListResponse>(
      `${API_BASE_URL}/api/Movies`
    );
  }
}