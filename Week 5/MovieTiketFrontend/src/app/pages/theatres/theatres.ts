import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { TheatreService } from '../../services/theatre.service';
import { ShowService } from '../../services/show.service';
import { Theatre } from '../../models/theatre.model';
import { Show } from '../../models/show.model';

@Component({
  selector: 'app-theatres',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './theatres.html',
  styleUrl: './theatres.css'
})
export class TheatresComponent implements OnInit {
  private theatreService = inject(TheatreService);
  private showService = inject(ShowService);

  theatres = signal<Theatre[]>([]);
  loading = signal<boolean>(true);
  errorMessage = signal<string>('');

  selectedTheatreId = signal<number | null>(null);
  theatreShows = signal<Show[]>([]);
  loadingShows = signal<boolean>(false);

  ngOnInit(): void {
    this.loadTheatres();
  }

  loadTheatres(): void {
    this.loading.set(true);
    this.errorMessage.set('');

    this.theatreService.getAll().subscribe({
      next: (list) => {
        this.theatres.set(list || []);
        this.loading.set(false);
      },
      error: (err) => {
        console.error('Failed to load theatres', err);
        this.errorMessage.set('Could not load theatres.');
        this.loading.set(false);
      }
    });
  }

  viewShowsForTheatre(theatreId: number): void {
    if (this.selectedTheatreId() === theatreId) {
      this.selectedTheatreId.set(null);
      this.theatreShows.set([]);
      return;
    }

    this.selectedTheatreId.set(theatreId);
    this.loadingShows.set(true);

    this.showService.getByTheatre(theatreId).subscribe({
      next: (shows) => {
        this.theatreShows.set(shows || []);
        this.loadingShows.set(false);
      },
      error: () => {
        this.loadingShows.set(false);
      }
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

