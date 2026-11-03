export interface Theatre {
  theatreId: number;
  name: string;
  address: string;
  city: string;
  state: string;
  phone?: string;
  email?: string;
  createdAt?: string;
  totalScreens?: number;
}

export interface CreateTheatreDto {
  name: string;
  address: string;
  city: string;
  state: string;
  phone?: string;
  email?: string;
}

export interface UpdateTheatreDto extends CreateTheatreDto {}

