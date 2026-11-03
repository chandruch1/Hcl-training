import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { API_BASE_URL } from '../config/api.config';
import {
  Booking,
  CreateBookingDto,
  CancelBookingResponse
} from '../models/booking.model';

@Injectable({
  providedIn: 'root'
})
export class BookingService {
  private http = inject(HttpClient);
  private apiUrl = `${API_BASE_URL}/api/Bookings`;

  private readonly RECENT_BOOKINGS_KEY = 'recent_booking_ids';

  createBooking(dto: CreateBookingDto): Observable<Booking> {
    return this.http.post<Booking>(this.apiUrl, dto).pipe(
      tap((b) => {
        if (b && b.bookingId) {
          this.saveLocalBookingId(b.bookingId);
        }
      })
    );
  }

  getBookingById(id: number): Observable<Booking> {
    return this.http.get<Booking>(`${this.apiUrl}/${id}`);
  }

  cancelBooking(id: number): Observable<CancelBookingResponse> {
    return this.http.post<CancelBookingResponse>(`${this.apiUrl}/${id}/cancel`, {});
  }

  saveLocalBookingId(id: number): void {
    const list = this.getRecentBookingIds();
    if (!list.includes(id)) {
      list.unshift(id);
      localStorage.setItem(this.RECENT_BOOKINGS_KEY, JSON.stringify(list.slice(0, 20)));
    }
  }

  getRecentBookingIds(): number[] {
    try {
      const stored = localStorage.getItem(this.RECENT_BOOKINGS_KEY);
      return stored ? JSON.parse(stored) : [];
    } catch {
      return [];
    }
  }
}

