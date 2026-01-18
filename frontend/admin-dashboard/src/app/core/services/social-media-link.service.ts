import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface SocialMediaLinkDto {
  id: string;
  platform: string;
  url: string;
  displayOrder: number;
  isActive: boolean;
}

export interface PaginatedResult<T> {
  value: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

@Injectable({ providedIn: 'root' })
export class SocialMediaLinkService {
  private http = inject(HttpClient);
  private baseUrl = `${environment.apiUrl}/SocialMediaLink`;

  getAll(
    platform?: string,
    isActive?: boolean,
    pageNumber = 1,
    pageSize = 10
  ): Observable<PaginatedResult<SocialMediaLinkDto>> {
    let params = new HttpParams()
      .set('PageNumber', pageNumber)
      .set('PageSize', pageSize);

    if (platform) params = params.set('Platform', platform);
    if (isActive !== undefined) params = params.set('IsActive', isActive);

    return this.http.get<PaginatedResult<SocialMediaLinkDto>>(this.baseUrl, { params, withCredentials: true });
  }

  getById(id: string): Observable<SocialMediaLinkDto> {
    return this.http.get<SocialMediaLinkDto>(`${this.baseUrl}/${id}`, { withCredentials: true });
  }

  create(
    platform: string,
    url: string,
    displayOrder: number,
    isActive: boolean
  ): Observable<string> {
    const payload = { platform, url, displayOrder, isActive };
    return this.http.post<string>(this.baseUrl, payload, { withCredentials: true });
  }

  update(link: SocialMediaLinkDto) {
    const payload = {
      id: link.id,
      platform: link.platform,
      url: link.url,
      displayOrder: link.displayOrder,
      isActive: link.isActive
    };

    return this.http.put(`${this.baseUrl}/${link.id}`, payload, { withCredentials: true });
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`, { withCredentials: true });
  }
}
