export interface Seat {
  seatId: number;
  screenId: number;
  screenName?: string;
  rowLetter: string;
  columnNumber: number;
  seatType: string;
  price: number;
}

export interface CreateSeatDto {
  screenId: number;
  rowLetter: string;
  columnNumber: number;
  seatType: string;
  price: number;
}

export interface BulkSeatItem {
  rowLetter: string;
  columnNumber: number;
  seatType: string;
  price: number;
}

export interface BulkCreateSeatDto {
  screenId: number;
  seats: BulkSeatItem[];
}

