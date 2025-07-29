import {inject, Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {environment} from '../../../environments/environment';

export interface SendContactMessageResponse {
  Message: string;
}

export interface SendContactMessageCommand {
  name: string;
  email: string;
  subject: string;
  message: string;
}

@Injectable({ providedIn: 'root' })
export class ContactService {
  private baseUrl = `${environment.apiUrl}/contact/send`;
  private http = inject(HttpClient);

  sendMessage(command: SendContactMessageCommand): Observable<SendContactMessageResponse> {
    return this.http.post<SendContactMessageResponse>(this.baseUrl, command);
  }
}
