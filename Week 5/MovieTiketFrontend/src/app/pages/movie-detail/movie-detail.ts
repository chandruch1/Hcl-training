import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { MovieService } from '../../services/movie.service';
import { ShowService } from '../../services/show.service';
import { Movie } from '../../models/movie.model';
import { Show } from '../../models/show.model';

interface TheatreShowsGroup {
  theatreId: number;
  theatreName: string;
  shows: Show[];
}

@Component({
  selector: 'app-movie-detail',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './movie-detail.html',
  styleUrl: './movie-detail.css'
})
export class MovieDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private movieService = inject(MovieService);
  private showService = inject(ShowService);

  movie = signal<Movie | null>(null);
  shows = signal<Show[]>([]);
  loadingMovie = signal<boolean>(true);
  loadingShows = signal<boolean>(true);
  errorMessage = signal<string>('');

  selectedDate = signal<string>('all');
  uniqueDates = signal<string[]>([]);

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (id) {
      this.loadMovie(id);
      this.loadShows(id);
    }
  }

  loadMovie(id: number): void {
    this.loadingMovie.set(true);
    this.movieService.getMovieById(id).subscribe({
      next: (m) => {
        this.movie.set(m);
        this.loadingMovie.set(false);
      },
      error: (err) => {
        console.error('Error loading movie', err);
        this.errorMessage.set('Failed to load movie details.');
        this.loadingMovie.set(false);
      }
    });
  }

  loadShows(movieId: number): void {
    this.loadingShows.set(true);
    this.showService.getByMovie(movieId).subscribe({
      next: (data) => {
        this.shows.set(data || []);
        // extract unique dates
        const dates = Array.from(new Set(data.map(s => this.formatDateOnly(s.showDate))));
        this.uniqueDates.set(dates);
        if (dates.length > 0) {
          this.selectedDate.set(dates[0]);
        }
        this.loadingShows.set(false);
      },
      error: (err) => {
        console.error('Error loading shows', err);
        this.loadingShows.set(false);
      }
    });
  }

  formatDateOnly(dateStr: string): string {
    if (!dateStr) return '';
    const d = new Date(dateStr);
    return d.toISOString().split('T')[0];
  }

  getFilteredTheatreGroups(): TheatreShowsGroup[] {
    const selDate = this.selectedDate();
    let currentShows = this.shows();

    if (selDate !== 'all') {
      currentShows = currentShows.filter(s => this.formatDateOnly(s.showDate) === selDate);
    }

    const groups: { [theatreId: number]: TheatreShowsGroup } = {};

    for (const show of currentShows) {
      if (!groups[show.theatreId]) {
        groups[show.theatreId] = {
          theatreId: show.theatreId,
          theatreName: show.theatreName || `Theatre #${show.theatreId}`,
          shows: []
        };
      }
      groups[show.theatreId].shows.push(show);
    }

    return Object.values(groups);
  }

  formatTime(timeStr: string): string {
    if (!timeStr) return '';
    const parts = timeStr.split(':');
    if (parts.length >= 2) {
      let hours = parseInt(parts[0], 10);
      const minutes = parts[1];
      const ampm = hours >= 12 ? 'PM' : 'AM';
      hours = hours % 12;
      hours = hours ? hours : 12;
      return `${hours}:${minutes} ${ampm}`;
    }
    return timeStr;
  }

  onImageError(event: Event): void {
    const target = event.target as HTMLImageElement;
    target.src = 'https://images.unsplash.com/photo-1489599849927-2ee91cede3ba?w=800&auto=format&fit=crop&q=80';
  }
}

