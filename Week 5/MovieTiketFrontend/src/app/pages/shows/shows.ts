import { Component, inject, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ShowService } from '../../services/show.service';
import { MovieService } from '../../services/movie.service';
import { TheatreService } from '../../services/theatre.service';
import { Show } from '../../models/show.model';
import { Movie } from '../../models/movie.model';
import { Theatre } from '../../models/theatre.model';

@Component({
  selector: 'app-shows',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './shows.html',
  styleUrl: './shows.css'
})
export class ShowsComponent implements OnInit {
  private showService = inject(ShowService);
  private movieService = inject(MovieService);
  private theatreService = inject(TheatreService);

  shows = signal<Show[]>([]);
  movies = signal<Movie[]>([]);
  theatres = signal<Theatre[]>([]);
  loading = signal<boolean>(true);
  errorMessage = signal<string>('');

  selectedMovieId = signal<number | 'all'>('all');
  selectedTheatreId = signal<number | 'all'>('all');

  filteredShows = computed(() => {
    let list = this.shows();
    const movieId = this.selectedMovieId();
    const theatreId = this.selectedTheatreId();

    if (movieId !== 'all') {
      list = list.filter(s => s.movieId === Number(movieId));
    }

    if (theatreId !== 'all') {
      list = list.filter(s => s.theatreId === Number(theatreId));
    }

    return list;
  });

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.loading.set(true);

    this.showService.getAll().subscribe({
      next: (data) => {
        this.shows.set(data || []);
        this.loading.set(false);
      },
      error: () => {
        this.errorMessage.set('Failed to load show schedules.');
        this.loading.set(false);
      }
    });

    this.movieService.getMovies(1, 100).subscribe({
      next: (res) => this.movies.set(res.movies || [])
    });

    this.theatreService.getAll().subscribe({
      next: (t) => this.theatres.set(t || [])
    });
  }

  formatTime(timeStr?: string): string {
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
}

