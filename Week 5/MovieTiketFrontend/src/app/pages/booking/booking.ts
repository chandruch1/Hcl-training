import { Component, inject, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ShowService } from '../../services/show.service';
import { SeatService } from '../../services/seat.service';
import { BookingService } from '../../services/booking.service';
import { AuthService } from '../../services/auth.service';
import { Show, ShowSeat } from '../../models/show.model';
import { CreateBookingDto } from '../../models/booking.model';

interface SeatRowGroup {
  rowLetter: string;
  seats: ShowSeat[];
}

@Component({
  selector: 'app-booking',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './booking.html',
  styleUrl: './booking.css'
})
export class BookingComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private showService = inject(ShowService);
  private seatService = inject(SeatService);
  private bookingService = inject(BookingService);
  authService = inject(AuthService);

  showId = signal<number>(0);
  show = signal<Show | null>(null);
  seats = signal<ShowSeat[]>([]);
  loading = signal<boolean>(true);
  bookingInProgress = signal<boolean>(false);
  errorMessage = signal<string>('');
  bookingError = signal<string>('');

  // Selected seat IDs
  selectedSeatIds = signal<number[]>([]);

  // Customer Contact Info
  customerName = signal<string>('');
  customerEmail = signal<string>('');
  customerPhone = signal<string>('');

  // Group seats by row
  seatRows = computed(() => {
    const list = this.seats();
    const map = new Map<string, ShowSeat[]>();

    for (const seat of list) {
      const row = seat.rowLetter || 'A';
      if (!map.has(row)) {
        map.set(row, []);
      }
      map.get(row)!.push(seat);
    }

    const groups: SeatRowGroup[] = [];
    const sortedRows = Array.from(map.keys()).sort();

    for (const row of sortedRows) {
      const rowSeats = map.get(row)!.sort((a, b) => a.columnNumber - b.columnNumber);
      groups.push({ rowLetter: row, seats: rowSeats });
    }

    return groups;
  });

  selectedSeatsList = computed(() => {
    const selectedIds = new Set(this.selectedSeatIds());
    return this.seats().filter(s => selectedIds.has(s.seatId));
  });

  subtotal = computed(() => {
    return this.selectedSeatsList().reduce((sum, s) => sum + (s.price || this.show()?.basePrice || 0), 0);
  });

  convenienceFee = computed(() => {
    return this.subtotal() > 0 ? Math.round(this.subtotal() * 0.1) : 0;
  });

  grandTotal = computed(() => {
    return this.subtotal() + this.convenienceFee();
  });

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('showId'));
    if (id) {
      this.showId.set(id);
      this.initCustomerDetails();
      this.loadShowAndSeats(id);
    }
  }

  private initCustomerDetails(): void {
    const user = this.authService.currentUser();
    if (user) {
      this.customerName.set(user.name);
      this.customerEmail.set(user.email);
    }
  }

  loadShowAndSeats(id: number): void {
    this.loading.set(true);
    this.errorMessage.set('');

    this.showService.getById(id).subscribe({
      next: (s) => {
        this.show.set(s);
        // Load show seats
        this.showService.getSeats(id).subscribe({
          next: (seatList) => {
            if (seatList && seatList.length > 0) {
              this.seats.set(seatList);
              this.loading.set(false);
            } else {
              // fallback to screen seats
              this.loadFallbackScreenSeats(s);
            }
          },
          error: () => {
            this.loadFallbackScreenSeats(s);
          }
        });
      },
      error: (err) => {
        console.error('Failed to load show', err);
        this.errorMessage.set('Show not found or unavailable.');
        this.loading.set(false);
      }
    });
  }

  private loadFallbackScreenSeats(show: Show): void {
    this.seatService.getByScreen(show.screenId).subscribe({
      next: (screenSeats) => {
        const mapped: ShowSeat[] = screenSeats.map(ss => ({
          showSeatId: ss.seatId,
          showId: show.showId,
          seatId: ss.seatId,
          rowLetter: ss.rowLetter,
          columnNumber: ss.columnNumber,
          seatType: ss.seatType,
          price: ss.price > 0 ? ss.price : show.basePrice,
          status: 'Available'
        }));
        this.seats.set(mapped);
        this.loading.set(false);
      },
      error: () => {
        this.errorMessage.set('Failed to load seat layout for this show.');
        this.loading.set(false);
      }
    });
  }

  toggleSeat(seat: ShowSeat): void {
    if (seat.status === 'Booked') return;

    const current = [...this.selectedSeatIds()];
    const index = current.indexOf(seat.seatId);

    if (index > -1) {
      current.splice(index, 1);
    } else {
      if (current.length >= 10) {
        alert('You can select a maximum of 10 seats per booking.');
        return;
      }
      current.push(seat.seatId);
    }

    this.selectedSeatIds.set(current);
  }

  isSeatSelected(seatId: number): boolean {
    return this.selectedSeatIds().includes(seatId);
  }

  confirmBooking(): void {
    this.bookingError.set('');

    if (this.selectedSeatIds().length === 0) {
      this.bookingError.set('Please select at least one seat to proceed.');
      return;
    }

    if (!this.customerName().trim() || !this.customerEmail().trim()) {
      this.bookingError.set('Please provide customer name and email.');
      return;
    }

    // Check email format
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(this.customerEmail())) {
      this.bookingError.set('Please enter a valid email address.');
      return;
    }

    this.bookingInProgress.set(true);

    const bookingDto: CreateBookingDto = {
      showId: this.showId(),
      seatIds: this.selectedSeatIds(),
      customerName: this.customerName().trim(),
      customerEmail: this.customerEmail().trim(),
      customerPhone: this.customerPhone().trim() || undefined
    };

    this.bookingService.createBooking(bookingDto).subscribe({
      next: (res) => {
        this.bookingInProgress.set(false);
        this.router.navigate(['/booking-confirmation', res.bookingId]);
      },
      error: (err) => {
        console.error('Booking failed', err);
        const msg = err.error?.message || err.error?.title || 'Booking failed. One or more selected seats may have already been booked.';
        this.bookingError.set(msg);
        this.bookingInProgress.set(false);
        // refresh seats
        this.loadShowAndSeats(this.showId());
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

