import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { BookingService } from '../../services/booking.service';
import { Booking } from '../../models/booking.model';
import { forkJoin, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Component({
  selector: 'app-my-bookings',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './my-bookings.html',
  styleUrl: './my-bookings.css'
})
export class MyBookingsComponent implements OnInit {
  private bookingService = inject(BookingService);

  searchBookingId = '';
  bookings = signal<Booking[]>([]);
  loading = signal<boolean>(false);
  errorMessage = signal<string>('');
  lookupMessage = signal<string>('');

  ngOnInit(): void {
    this.loadRecentBookings();
  }

  loadRecentBookings(): void {
    const ids = this.bookingService.getRecentBookingIds();
    if (ids.length === 0) {
      return;
    }

    this.loading.set(true);
    const requests = ids.map(id =>
      this.bookingService.getBookingById(id).pipe(
        catchError(() => of(null))
      )
    );

    forkJoin(requests).subscribe({
      next: (results) => {
        const valid = results.filter((b): b is Booking => b !== null);
        this.bookings.set(valid);
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
      }
    });
  }

  onLookupSubmit(): void {
    this.lookupMessage.set('');
    const id = parseInt(this.searchBookingId.trim(), 10);
    if (isNaN(id) || id <= 0) {
      this.lookupMessage.set('Please enter a valid numeric Booking ID.');
      return;
    }

    this.loading.set(true);
    this.bookingService.getBookingById(id).subscribe({
      next: (b) => {
        this.loading.set(false);
        this.bookingService.saveLocalBookingId(b.bookingId);
        // Replace or add to top
        const existing = this.bookings().filter(x => x.bookingId !== b.bookingId);
        this.bookings.set([b, ...existing]);
      },
      error: () => {
        this.loading.set(false);
        this.lookupMessage.set(`No booking found with ID #${id}.`);
      }
    });
  }

  cancelBooking(id: number): void {
    if (!confirm(`Are you sure you want to cancel booking #${id}?`)) {
      return;
    }

    this.bookingService.cancelBooking(id).subscribe({
      next: (res) => {
        alert(res.message || 'Booking cancelled successfully.');
        this.loadRecentBookings();
      },
      error: (err) => {
        alert(err.error?.message || 'Failed to cancel booking.');
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

