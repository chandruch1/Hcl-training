import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormsModule, ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { MovieService } from '../../services/movie.service';
import { TheatreService } from '../../services/theatre.service';
import { ScreenService } from '../../services/screen.service';
import { SeatService } from '../../services/seat.service';
import { ShowService } from '../../services/show.service';
import { Movie, CreateMovieDto, UpdateMovieDto } from '../../models/movie.model';
import { Theatre, CreateTheatreDto } from '../../models/theatre.model';
import { Screen, CreateScreenDto } from '../../models/screen.model';
import { BulkCreateSeatDto, BulkSeatItem } from '../../models/seat.model';
import { Show, CreateShowDto } from '../../models/show.model';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './admin-dashboard.html',
  styleUrl: './admin-dashboard.css'
})
export class AdminDashboard implements OnInit {
  authService = inject(AuthService);
  private router = inject(Router);
  private fb = inject(FormBuilder).nonNullable;
  private movieService = inject(MovieService);
  private theatreService = inject(TheatreService);
  private screenService = inject(ScreenService);
  private seatService = inject(SeatService);
  private showService = inject(ShowService);

  activeTab = signal<'overview' | 'movies' | 'theatres' | 'shows'>('overview');

  // Data signals
  movies = signal<Movie[]>([]);
  theatres = signal<Theatre[]>([]);
  screens = signal<Screen[]>([]);
  shows = signal<Show[]>([]);

  loading = signal<boolean>(false);
  actionMessage = signal<string>('');
  actionError = signal<string>('');

  // Modals visibility
  showMovieModal = signal<boolean>(false);
  editingMovieId = signal<number | null>(null);

  showTheatreModal = signal<boolean>(false);
  showScreenModal = signal<boolean>(false);
  showBulkSeatModal = signal<boolean>(false);
  showScheduleModal = signal<boolean>(false);

  // Forms
  movieForm = this.fb.group({
    title: ['', Validators.required],
    description: ['', Validators.required],
    genre: ['', Validators.required],
    language: ['English', Validators.required],
    durationMinutes: [120, [Validators.required, Validators.min(1)]],
    releaseDate: [new Date().toISOString().split('T')[0], Validators.required],
    rating: [8.0, [Validators.required, Validators.min(0), Validators.max(10)]],
    posterUrl: ['', Validators.required],
    isActive: [true]
  });

  theatreForm = this.fb.group({
    name: ['', Validators.required],
    address: ['', Validators.required],
    city: ['Chennai', Validators.required],
    state: ['Tamil Nadu', Validators.required],
    phone: ['+91-9876543210'],
    email: ['theatre@cinepass.com', [Validators.email]]
  });

  screenForm = this.fb.group({
    theatreId: [0, [Validators.required, Validators.min(1)]],
    screenName: ['Screen 1 (IMAX)', Validators.required],
    totalSeats: [60, [Validators.required, Validators.min(1)]],
    screenType: ['IMAX', Validators.required]
  });

  bulkSeatForm = this.fb.group({
    screenId: [0, [Validators.required, Validators.min(1)]],
    regularRows: ['A,B,C,D', Validators.required],
    regularCols: [10, [Validators.required, Validators.min(1)]],
    regularPrice: [180, [Validators.required, Validators.min(1)]],
    vipRows: ['E,F', Validators.required],
    vipCols: [10, [Validators.required, Validators.min(1)]],
    vipPrice: [250, [Validators.required, Validators.min(1)]]
  });

  showForm = this.fb.group({
    movieId: [0, [Validators.required, Validators.min(1)]],
    screenId: [0, [Validators.required, Validators.min(1)]],
    showDate: [new Date().toISOString().split('T')[0], Validators.required],
    startTime: ['18:30', Validators.required],
    basePrice: [200, [Validators.required, Validators.min(1)]]
  });

  ngOnInit(): void {
    this.refreshAll();
  }

  refreshAll(): void {
    this.loading.set(true);
    this.actionMessage.set('');
    this.actionError.set('');

    this.movieService.getMovies(1, 100).subscribe({
      next: (res) => this.movies.set(res.movies || []),
      error: (e) => console.error(e)
    });

    this.theatreService.getAll().subscribe({
      next: (t) => this.theatres.set(t || []),
      error: (e) => console.error(e)
    });

    this.screenService.getAll().subscribe({
      next: (s) => this.screens.set(s || []),
      error: (e) => console.error(e)
    });

    this.showService.getAll().subscribe({
      next: (s) => {
        this.shows.set(s || []);
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }

  // --- MOVIES CRUD ---
  openAddMovieModal(): void {
    this.editingMovieId.set(null);
    this.movieForm.reset({
      title: '',
      description: '',
      genre: 'Action / Sci-Fi',
      language: 'English',
      durationMinutes: 148,
      releaseDate: new Date().toISOString().split('T')[0],
      rating: 8.5,
      posterUrl: 'https://images.unsplash.com/photo-1536440136628-849c177e76a1?w=800',
      isActive: true
    });
    this.showMovieModal.set(true);
  }

  openEditMovieModal(movie: Movie): void {
    this.editingMovieId.set(movie.movieId);
    this.movieForm.patchValue({
      title: movie.title,
      description: movie.description,
      genre: movie.genre,
      language: movie.language,
      durationMinutes: movie.durationMinutes,
      releaseDate: movie.releaseDate ? movie.releaseDate.split('T')[0] : '',
      rating: movie.rating,
      posterUrl: movie.posterUrl,
      isActive: movie.isActive
    });
    this.showMovieModal.set(true);
  }

  saveMovie(): void {
    if (this.movieForm.invalid) return;

    const val = this.movieForm.getRawValue();
    const isEdit = this.editingMovieId() !== null;

    if (isEdit) {
      const dto: UpdateMovieDto = {
        title: val.title,
        description: val.description,
        genre: val.genre,
        language: val.language,
        durationMinutes: val.durationMinutes,
        releaseDate: new Date(val.releaseDate).toISOString(),
        rating: val.rating,
        posterUrl: val.posterUrl,
        isActive: val.isActive
      };

      this.movieService.updateMovie(this.editingMovieId()!, dto).subscribe({
        next: () => {
          this.actionMessage.set('Movie updated successfully!');
          this.showMovieModal.set(false);
          this.refreshAll();
        },
        error: (err) => this.actionError.set(err.error?.message || 'Failed to update movie.')
      });
    } else {
      const dto: CreateMovieDto = {
        title: val.title,
        description: val.description,
        genre: val.genre,
        language: val.language,
        durationMinutes: val.durationMinutes,
        releaseDate: new Date(val.releaseDate).toISOString(),
        rating: val.rating,
        posterUrl: val.posterUrl
      };

      this.movieService.createMovie(dto).subscribe({
        next: () => {
          this.actionMessage.set('Movie added successfully!');
          this.showMovieModal.set(false);
          this.refreshAll();
        },
        error: (err) => this.actionError.set(err.error?.message || 'Failed to create movie.')
      });
    }
  }

  deleteMovie(id: number): void {
    if (!confirm('Are you sure you want to delete this movie?')) return;

    this.movieService.deleteMovie(id).subscribe({
      next: () => {
        this.actionMessage.set('Movie deleted successfully!');
        this.refreshAll();
      },
      error: (err) => this.actionError.set(err.error?.message || 'Failed to delete movie.')
    });
  }

  // --- THEATRES CRUD ---
  openAddTheatreModal(): void {
    this.theatreForm.reset({
      name: '',
      address: '',
      city: 'Chennai',
      state: 'Tamil Nadu',
      phone: '+91-9876543210',
      email: 'chennai@pvrcinemas.com'
    });
    this.showTheatreModal.set(true);
  }

  saveTheatre(): void {
    if (this.theatreForm.invalid) return;

    const dto: CreateTheatreDto = this.theatreForm.getRawValue();
    this.theatreService.create(dto).subscribe({
      next: () => {
        this.actionMessage.set('Theatre registered successfully!');
        this.showTheatreModal.set(false);
        this.refreshAll();
      },
      error: (err) => this.actionError.set(err.error?.message || 'Failed to register theatre.')
    });
  }

  deleteTheatre(id: number): void {
    if (!confirm('Are you sure you want to delete this theatre?')) return;

    this.theatreService.delete(id).subscribe({
      next: () => {
        this.actionMessage.set('Theatre deleted successfully.');
        this.refreshAll();
      },
      error: (err) => this.actionError.set(err.error?.message || 'Failed to delete theatre.')
    });
  }

  // --- SCREENS CRUD ---
  openAddScreenModal(theatreId?: number): void {
    const tId = theatreId || (this.theatres().length > 0 ? this.theatres()[0].theatreId : 0);
    this.screenForm.patchValue({
      theatreId: tId,
      screenName: 'Screen 1 (IMAX)',
      totalSeats: 60,
      screenType: 'IMAX'
    });
    this.showScreenModal.set(true);
  }

  saveScreen(): void {
    if (this.screenForm.invalid) return;

    const val = this.screenForm.getRawValue();
    const dto: CreateScreenDto = {
      theatreId: Number(val.theatreId),
      screenName: val.screenName,
      totalSeats: val.totalSeats,
      screenType: val.screenType
    };

    this.screenService.create(dto).subscribe({
      next: (created) => {
        this.actionMessage.set(`Screen '${created.screenName}' created! You can now generate seats for it.`);
        this.showScreenModal.set(false);
        this.refreshAll();
      },
      error: (err) => this.actionError.set(err.error?.message || 'Failed to create screen.')
    });
  }

  deleteScreen(id: number): void {
    if (!confirm('Are you sure you want to delete this screen?')) return;

    this.screenService.delete(id).subscribe({
      next: () => {
        this.actionMessage.set('Screen removed successfully.');
        this.refreshAll();
      },
      error: (err) => this.actionError.set(err.error?.message || 'Failed to delete screen.')
    });
  }

  // --- BULK SEAT GENERATOR ---
  openBulkSeatModal(screenId?: number): void {
    const sId = screenId || (this.screens().length > 0 ? this.screens()[0].screenId : 0);
    this.bulkSeatForm.patchValue({
      screenId: sId,
      regularRows: 'A,B,C,D',
      regularCols: 10,
      regularPrice: 180,
      vipRows: 'E,F',
      vipCols: 10,
      vipPrice: 250
    });
    this.showBulkSeatModal.set(true);
  }

  generateBulkSeats(): void {
    if (this.bulkSeatForm.invalid) return;

    const val = this.bulkSeatForm.getRawValue();
    const screenId = Number(val.screenId);
    const seats: BulkSeatItem[] = [];

    // Process Regular rows
    const regRows = val.regularRows.split(',').map(r => r.trim().toUpperCase()).filter(r => r);
    for (const r of regRows) {
      for (let col = 1; col <= val.regularCols; col++) {
        seats.push({
          rowLetter: r,
          columnNumber: col,
          seatType: 'Regular',
          price: val.regularPrice
        });
      }
    }

    // Process VIP rows
    const vipRows = val.vipRows.split(',').map(r => r.trim().toUpperCase()).filter(r => r);
    for (const r of vipRows) {
      for (let col = 1; col <= val.vipCols; col++) {
        seats.push({
          rowLetter: r,
          columnNumber: col,
          seatType: 'VIP',
          price: val.vipPrice
        });
      }
    }

    const bulkDto: BulkCreateSeatDto = {
      screenId,
      seats
    };

    this.seatService.createBulk(bulkDto).subscribe({
      next: (res) => {
        this.actionMessage.set(`Successfully created ${res.length} seats for Screen #${screenId}!`);
        this.showBulkSeatModal.set(false);
      },
      error: (err) => this.actionError.set(err.error?.message || 'Bulk seat creation failed.')
    });
  }

  // --- SHOWS CRUD ---
  openAddScheduleModal(): void {
    const defaultMovieId = this.movies().length > 0 ? this.movies()[0].movieId : 0;
    const defaultScreenId = this.screens().length > 0 ? this.screens()[0].screenId : 0;

    this.showForm.patchValue({
      movieId: defaultMovieId,
      screenId: defaultScreenId,
      showDate: new Date().toISOString().split('T')[0],
      startTime: '18:30',
      basePrice: 200
    });
    this.showScheduleModal.set(true);
  }

  saveSchedule(): void {
    if (this.showForm.invalid) return;

    const val = this.showForm.getRawValue();
    const timeWithSeconds = val.startTime.length === 5 ? `${val.startTime}:00` : val.startTime;

    const dto: CreateShowDto = {
      movieId: Number(val.movieId),
      screenId: Number(val.screenId),
      showDate: new Date(val.showDate).toISOString(),
      startTime: timeWithSeconds,
      basePrice: Number(val.basePrice)
    };

    this.showService.create(dto).subscribe({
      next: () => {
        this.actionMessage.set('Show scheduled successfully! Show seats were generated.');
        this.showScheduleModal.set(false);
        this.refreshAll();
      },
      error: (err) => {
        this.actionError.set(
          err.error?.message || 'Failed to schedule show. Check for screen time overlap or ensure screen has seats.'
        );
      }
    });
  }

  deleteShow(id: number): void {
    if (!confirm('Are you sure you want to cancel and remove this show schedule?')) return;

    this.showService.delete(id).subscribe({
      next: () => {
        this.actionMessage.set('Show schedule removed.');
        this.refreshAll();
      },
      error: (err) => this.actionError.set(err.error?.message || 'Failed to delete show.')
    });
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/admin/login']);
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
