export interface Movie {
  movieId: number;
  title: string;
  description: string;
  genre: string;
  language: string;
  durationMinutes: number;
  releaseDate: string;
  rating: number;
  posterUrl: string;
  isActive: boolean;
  createdAt?: string;
}

export interface CreateMovieDto {
  title: string;
  description: string;
  genre: string;
  language: string;
  durationMinutes: number;
  releaseDate: string;
  rating: number;
  posterUrl: string;
}

export interface UpdateMovieDto extends CreateMovieDto {
  isActive: boolean;
}

export interface MovieSummary {
  movieId: number;
  title: string;
  genre: string;
  language: string;
  durationMinutes: number;
  rating: number;
  posterUrl: string;
  isActive: boolean;
}

export interface PagedMovieResponse {
  movies: Movie[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}

