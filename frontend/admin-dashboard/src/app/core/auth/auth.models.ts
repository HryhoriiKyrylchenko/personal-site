export interface LoginResponse {
  mustChangePassword: boolean;
}

export interface CurrentUser {
  id: string;
  username: string;
  role: string;
  mustChangePassword: boolean;
}
