export interface Screen {
  screenId: number;
  theatreId: number;
  theatreName?: string;
  screenName: string;
  totalSeats: number;
  screenType: string;
  createdAt?: string;
}

export interface CreateScreenDto {
  theatreId: number;
  screenName: string;
  totalSeats: number;
  screenType: string;
}

export interface UpdateScreenDto {
  screenName: string;
  totalSeats: number;
  screenType: string;
}

