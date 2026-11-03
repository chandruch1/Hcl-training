import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { BookingService } from '../../services/booking.service';
import { Booking } from '../../models/booking.model';

@Component({
  selector: 'app-booking-confirmation',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './booking-confirmation.html',
  styleUrl: './booking-confirmation.css'
})
export class BookingConfirmationComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private bookingService = inject(BookingService);

  booking = signal<Booking | null>(null);
  loading = signal<boolean>(true);
  errorMessage = signal<string>('');
  cancelling = signal<boolean>(false);
  cancelSuccessMessage = signal<string>('');

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('bookingId'));
    if (id) {
      this.loadBooking(id);
    }
  }

  loadBooking(id: number): void {
    this.loading.set(true);
    this.errorMessage.set('');

    this.bookingService.getBookingById(id).subscribe({
      next: (b) => {
        this.booking.set(b);
        this.loading.set(false);
      },
      error: (err) => {
        console.error('Error loading booking', err);
        this.errorMessage.set('Could not find booking information.');
        this.loading.set(false);
      }
    });
  }

  cancelBooking(): void {
    const b = this.booking();
    if (!b) return;

    if (!confirm(`Are you sure you want to cancel booking #${b.bookingId}? This will release your reserved seats.`)) {
      return;
    }

    this.cancelling.set(true);
    this.bookingService.cancelBooking(b.bookingId).subscribe({
      next: (res) => {
        this.cancelling.set(false);
        this.cancelSuccessMessage.set(res.message || 'Booking cancelled successfully.');
        // Refresh booking details
        this.loadBooking(b.bookingId);
      },
      error: (err) => {
        console.error('Cancel booking failed', err);
        alert(err.error?.message || 'Failed to cancel booking.');
        this.cancelling.set(false);
      }
    });
  }

  printTicket(): void {
    window.print();
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

