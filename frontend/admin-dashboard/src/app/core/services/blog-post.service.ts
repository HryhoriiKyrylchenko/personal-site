import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface BlogPostTranslationDto {
  id?: string;
  languageCode: string;
  blogPostId?: string;
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

  getAll(page = 1, pageSize = 10, slugFilter?: string, isPublishedFilter?: boolean)
    : Observable<PaginatedResult<BlogPostAdminDto>> {
    let params = new HttpParams()
      .set('Page', page)
      .set('PageSize', pageSize);

    if (slugFilter) params = params.set('SlugFilter', slugFilter);
    if (isPublishedFilter !== undefined) params = params.set('IsPublishedFilter', isPublishedFilter);

    return this.http.get<PaginatedResult<BlogPostAdminDto>>(this.baseUrl, { params, withCredentials: true });
  }

  getById(id: string) {
    return this.http.get<BlogPostAdminDto>(`${this.baseUrl}/${id}`, { withCredentials: true });
  }

  create(post: BlogPostAdminDto) {
    const payload = {
      slug: post.slug,
      coverImage: post.coverImage,
      isPublished: post.isPublished,
      translations: post.translations.map(t => ({
        id: t.id || '00000000-0000-0000-0000-000000000000', // optional placeholder
        languageCode: t.languageCode,
        blogPostId: post.id || '00000000-0000-0000-0000-000000000000',
        title: t.title,
        excerpt: t.excerpt,
        content: t.content,
        metaTitle: t.metaTitle,
        metaDescription: t.metaDescription,
        ogImage: t.ogImage
      })),
      tags: post.tags.map(tag => ({
        id: tag.id || '00000000-0000-0000-0000-000000000000',
        name: tag.name
      }))
    };

    return this.http.post<string>(this.baseUrl, payload, { withCredentials: true });
  }

  update(id: string, post: BlogPostAdminDto) {
    const payload = {
      id,
      slug: post.slug,
      coverImage: post.coverImage,
      isDeleted: post.isDeleted,
      translations: post.translations.map(t => ({
        id: t.id || '00000000-0000-0000-0000-000000000000',
        languageCode: t.languageCode,
        blogPostId: id,
        title: t.title,
        excerpt: t.excerpt,
        content: t.content,
        metaTitle: t.metaTitle,
        metaDescription: t.metaDescription,
        ogImage: t.ogImage
      })),
      tags: post.tags.map(tag => ({
        id: tag.id || '00000000-0000-0000-0000-000000000000',
        name: tag.name
      }))
    };

    return this.http.put(`${this.baseUrl}/${id}`, payload, { withCredentials: true });
  }

  delete(id: string) {
    return this.http.delete(`${this.baseUrl}/${id}`, { withCredentials: true });
  }

  publish(id: string, isPublished: boolean, publishDate?: string) {
    return this.http.post(`${this.baseUrl}/${id}/publish`, { id, isPublished, publishDate }, { withCredentials: true });
  }
}
