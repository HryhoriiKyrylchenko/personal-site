import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface LanguageDto {
  id: string;
  code: string;
  name: string;
}

@Injectable({ providedIn: 'root' })
export class LanguageService {
  private http = inject(HttpClient);
  private baseUrl = `${environment.apiUrl}/Language`;

  getAll(): Observable<LanguageDto[]> {
    return this.http.get<LanguageDto[]>(this.baseUrl);
  }

  getById(id: string): Observable<LanguageDto> {
    return this.http.get<LanguageDto>(`${this.baseUrl}/${id}`);
  }

  create(code: string, name: string): Observable<string> {
    const payload = {
      code,
      name
    };

    return this.http.post<string>(this.baseUrl, payload);
  }

  update(language: LanguageDto) {
    const payload = {
      id: language.id,
      code: language.code,
      name: language.name
    };

    return this.http.put(`${this.baseUrl}/${language.id}`, payload);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
