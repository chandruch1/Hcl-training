export interface LoginRequest {
  email: string;
  passWord: string;
}

export interface RegisterRequest {
  userName: string;
  email: string;
  passWord: string;
}

export interface AdminRegisterRequest {
  userName: string;
  email: string;
  passWord: string;
}

export interface AuthResponse {
  message?: string;
  Message?: string;
  accessToken?: string;
  accesToken?: string;
  adminId?: number;
  userId?: number;
  Username?: number | string;
  username?: string;
  name?: string;
  email?: string;
  Email?: string;
  role?: string | number;
  Role?: string | number;
}

export interface CurrentUser {
  token: string;
  userId?: string | number;
  name: string;
  email: string;
  role: 'Admin' | 'Customer' | 'Owner' | string;
}

