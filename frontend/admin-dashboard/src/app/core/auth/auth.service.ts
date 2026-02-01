import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../environments/environment';
import {CurrentUser, LoginResponse} from './auth.models';
import {Observable} from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private baseUrl = `${environment.apiUrl}/Auth`;
  private http = inject(HttpClient);

  login(username: string, password: string) {
    return this.http.post<LoginResponse>(
      `${this.baseUrl}/login`,
      { username, password },
      { withCredentials: true }
    );
  }

  logout() {
    return this.http.post(
      `${this.baseUrl}/logout`,
      {},
      { withCredentials: true }
    );
  }

  changePassword(
    currentPassword: string,
    newPassword: string
  ): Observable<void> {
    return this.http.post<void>(
      `${this.baseUrl}/change-password`,
      { currentPassword, newPassword },
      { withCredentials: true }
    );
  }

  me() {
    return this.http.get<CurrentUser>(
      `${this.baseUrl}/me`,
      { withCredentials: true }
    );
  }
}
