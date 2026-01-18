import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {map, Observable} from 'rxjs';
import { environment } from '../../../environments/environment';

export interface PageTranslationDto {
  id?: string;
  languageCode: string;
  pageId?: string;
  data: string;
  title: string;
  description: string;
  metaTitle: string;
  metaDescription: string;
  ogImage: string;
}

export interface PageAdminDto {
  id?: string;
  key: string;
  translations: PageTranslationDto[];
}

@Injectable({ providedIn: 'root' })
export class PageService {
  private http = inject(HttpClient);
  private baseUrl = `${environment.apiUrl}/Page`;

  getAll(): Observable<PageAdminDto[]> {
    return this.http.get<PageAdminDto[]>(this.baseUrl, { withCredentials: true }).pipe(
      map(res => {
        res.forEach(page =>
          page.translations.forEach(t =>
            t.data = JSON.stringify(t.data)
          )
        );
        return res;
      })
    );
  }

  getById(id: string): Observable<PageAdminDto> {
    return this.http.get<PageAdminDto>(`${this.baseUrl}/${id}`, { withCredentials: true });
  }

  create(page: PageAdminDto): Observable<string> {
    const payload = {
      Key: page.key,
      Translations: page.translations.map(t => ({
        id: '00000000-0000-0000-0000-000000000000',
        languageCode: t.languageCode,
        pageId: '00000000-0000-0000-0000-000000000000',
        data: JSON.parse(t.data),
        title: t.title,
        description: t.description,
        metaTitle: t.metaTitle,
        metaDescription: t.metaDescription,
        ogImage: t.ogImage
      }))
    };

    return this.http.post<string>(this.baseUrl, payload, { withCredentials: true });
  }

  update(page: PageAdminDto) {
    const payload = {
      Id: page.id,
      Key: page.key,
      Translations: page.translations.map(t => ({
        id: t.id ?? '00000000-0000-0000-0000-000000000000',
        languageCode: t.languageCode,
        pageId: page.id,
        data: JSON.parse(t.data),
        title: t.title,
        description: t.description,
        metaTitle: t.metaTitle,
        metaDescription: t.metaDescription,
        ogImage: t.ogImage
      }))
    };

    return this.http.put(`${this.baseUrl}/${page.id}`, payload, { withCredentials: true });
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`, { withCredentials: true });
  }
}
