export interface CreateBookingDto {
  showId: number;
  seatIds: number[];
  customerName: string;
  customerEmail: string;
  customerPhone?: string;
}

export interface BookedSeat {
  seatId: number;
  seatNumber: string;
  seatType: string;
  price: number;
}

export interface Booking {
  bookingId: number;
  showId: number;
  movieTitle: string;
  theatreName: string;
  screenName: string;
  showDate: string;
  startTime: string;
  customerName: string;
  customerEmail: string;
  customerPhone?: string;
  totalAmount: number;
  bookingStatus: 'Confirmed' | 'Cancelled' | string;
  bookingDate: string;
  bookedSeats: BookedSeat[];
}

export interface CancelBookingResponse {
  bookingId: number;
  bookingStatus: string;
  message: string;
  cancelledAt: string;
}

