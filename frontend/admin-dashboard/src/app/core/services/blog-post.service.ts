import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface BlogPostTranslationDto {
  languageCode: string;
  title: string;
  excerpt: string;
  content: string;
  metaTitle: string;
  metaDescription: string;
  ogImage: string;
}

export interface BlogPostTagDto {
  id: string;
  name: string;
}

export interface BlogPostAdminDto {
  id: string;
  slug: string;
  coverImage: string;
  createdAt: string;
  updatedAt?: string;
  isDeleted: boolean;
  isPublished: boolean;
  publishedAt?: string;
  translations: BlogPostTranslationDto[];
  tags: BlogPostTagDto[];
}

export interface PaginatedResult<T> {
  value: T[];
  isSuccess: boolean;
  error?: string;
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

@Injectable({ providedIn: 'root' })
export class BlogPostService {
  private http = inject(HttpClient);
  private baseUrl = `${environment.apiUrl}/BlogPost`;

  getAll(page = 1, pageSize = 10, slugFilter?: string, isPublishedFilter?: boolean): Observable<PaginatedResult<BlogPostAdminDto>> {
    let params = new HttpParams()
      .set('Page', page)
      .set('PageSize', pageSize);

    if (slugFilter) params = params.set('SlugFilter', slugFilter);
    if (isPublishedFilter !== undefined) params = params.set('IsPublishedFilter', isPublishedFilter);

    return this.http.get<PaginatedResult<BlogPostAdminDto>>(this.baseUrl, { params });
  }

  getById(id: string) {
    return this.http.get<BlogPostAdminDto>(`${this.baseUrl}/${id}`);
  }

  create(data: any) {
    return this.http.post<string>(this.baseUrl, data);
  }

  update(id: string, data: any) {
    return this.http.put(`${this.baseUrl}/${id}`, data);
  }

  delete(id: string) {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }

  publish(id: string, isPublished: boolean, publishDate?: string) {
    return this.http.post(`${this.baseUrl}/${id}/publish`, { id, isPublished, publishDate });
  }
}
