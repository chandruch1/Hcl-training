export interface AdminLoginResponse {
  message: string;
  accessToken: string;
  adminId: number;
  name: string;
  email: string;
  role: string;
}