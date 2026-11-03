import { Component, inject, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MovieService } from '../../services/movie.service';
import { Movie } from '../../models/movie.model';

@Component({
  selector: 'app-movie-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './movie-list.html',
  styleUrl: './movie-list.css'
})
export class MovieListComponent implements OnInit {
  private movieService = inject(MovieService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  movies = signal<Movie[]>([]);
  loading = signal<boolean>(true);
  errorMessage = signal<string>('');

  searchQuery = signal<string>('');
  selectedGenre = signal<string>('All');
  selectedLanguage = signal<string>('All');

  currentPage = signal<number>(1);
  pageSize = signal<number>(12);
  totalPages = signal<number>(1);
  totalCount = signal<number>(0);

  // Available genres & languages collected from movies
  genres = computed(() => {
    const set = new Set<string>();
    this.movies().forEach(m => {
      if (m.genre) {
        m.genre.split(/[\/,]/).forEach(g => set.add(g.trim()));
      }
    });
    return ['All', ...Array.from(set).sort()];
  });

  languages = computed(() => {
    const set = new Set<string>();
    this.movies().forEach(m => {
      if (m.language) set.add(m.language.trim());
    });
    return ['All', ...Array.from(set).sort()];
  });

  filteredMovies = computed(() => {
    let list = this.movies();
    const query = this.searchQuery().trim().toLowerCase();
    const genre = this.selectedGenre();
    const lang = this.selectedLanguage();

    if (query) {
      list = list.filter(m =>
        m.title.toLowerCase().includes(query) ||
        (m.description && m.description.toLowerCase().includes(query)) ||
        (m.genre && m.genre.toLowerCase().includes(query))
      );
    }

    if (genre !== 'All') {
      list = list.filter(m => m.genre && m.genre.toLowerCase().includes(genre.toLowerCase()));
    }

    if (lang !== 'All') {
      list = list.filter(m => m.language && m.language.toLowerCase() === lang.toLowerCase());
    }

    return list;
  });

  featuredMovie = computed(() => {
    const list = this.movies();
    return list.length > 0 ? list[0] : null;
  });

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      if (params['q']) {
        this.searchQuery.set(params['q']);
      }
    });
    this.loadMovies();
  }

  loadMovies(): void {
    this.loading.set(true);
    this.errorMessage.set('');

    this.movieService.getMovies(this.currentPage(), this.pageSize()).subscribe({
      next: (res) => {
        this.movies.set(res.movies || []);
        this.totalCount.set(res.totalCount || res.movies?.length || 0);
        this.totalPages.set(res.totalPages || Math.ceil((res.totalCount || res.movies?.length || 1) / this.pageSize()));
        this.loading.set(false);
      },
      error: (err) => {
        console.error('Failed to load movies', err);
        this.errorMessage.set('Could not connect to the movie API service. Please verify the backend is running.');
        this.loading.set(false);
      }
    });
  }

  setGenre(genre: string): void {
    this.selectedGenre.set(genre);
  }

  setLanguage(lang: string): void {
    this.selectedLanguage.set(lang);
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages() && page !== this.currentPage()) {
      this.currentPage.set(page);
      this.loadMovies();
      window.scrollTo({ top: 400, behavior: 'smooth' });
    }
  }

  clearFilters(): void {
    this.searchQuery.set('');
    this.selectedGenre.set('All');
    this.selectedLanguage.set('All');
  }

  onImageError(event: Event): void {
    const target = event.target as HTMLImageElement;
    target.src = 'https://images.unsplash.com/photo-1489599849927-2ee91cede3ba?w=600&auto=format&fit=crop&q=80';
  }
}

