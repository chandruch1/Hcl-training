export interface Show {
  showId: number;
  movieId: number;
  movieTitle: string;
  durationMinutes: number;
  screenId: number;
  screenName: string;
  theatreId: number;
  theatreName: string;
  showDate: string;
  startTime: string;
  endTime: string;
  basePrice: number;
  isActive: boolean;
  createdAt: string;
  totalSeats: number;
  availableSeats: number;
}

export interface CreateShowDto {
  movieId: number;
  screenId: number;
  showDate: string; // ISO date string YYYY-MM-DD
  startTime: string; // HH:mm:ss
  basePrice: number;
}

export interface UpdateShowDto {
  showDate: string;
  startTime: string;
  basePrice: number;
  isActive: boolean;
}

export interface ShowSeat {
  showSeatId: number;
  showId: number;
  seatId: number;
  rowLetter: string;
  columnNumber: number;
  seatType: string;
  price: number;
  status: 'Available' | 'Booked' | string;
}

