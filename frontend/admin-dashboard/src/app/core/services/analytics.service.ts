import {inject, Injectable} from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import {map, Observable} from 'rxjs';
import {environment} from '../../../environments/environment';

export interface AnalyticsEventDto {
  id: string;
  eventType: string;
  pageSlug: string;
  referrer?: string;
  userAgent?: string;
  createdAt: string;
  additionalDataJson?: string;
}

export interface PaginatedResult<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

@Injectable({ providedIn: 'root' })
export class AnalyticsService {
  private baseUrl = `${environment.apiUrl}/AnalyticsEvent`;
  private http = inject(HttpClient);

  fetchEvents(
    page: number,
    pageSize: number,
    eventType?: string,
    pageSlug?: string,
    from?: string,
    to?: string
  ): Observable<PaginatedResult<AnalyticsEventDto>> {
    let params = new HttpParams()
      .set('Page', String(page))
      .set('PageSize', String(pageSize));

    if (eventType) params = params.set('EventType', eventType);
    if (pageSlug) params = params.set('PageSlug', pageSlug);
    if (from) {
      const fromDate = new Date(from);
      params = params.set('From', fromDate.toISOString());
    }
    if (to) {
      const toDate = new Date(to);
      toDate.setHours(23, 59, 59, 999);
      params = params.set('To', toDate.toISOString());
    }

    return this.http.get<any>(this.baseUrl, { params }).pipe(
      map(res => ({
        ...res,
        items: res.value.map((x: any) => ({ ...x, createdAt: new Date(x.createdAt) }))
      }))
    );
  }

  deleteEvent(id: string) {
    return this.http.delete<void>(`${this.baseUrl}/${id}`, { body: { id } });
  }

  deleteEventsRange(ids: string[]) {
    return this.http.post<void>(`${this.baseUrl}/range`, { ids });
  }
}
