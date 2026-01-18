import {inject, Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {map, Observable} from 'rxjs';
import {environment} from '../../../environments/environment';
import {PaginatedResult} from './analytics.service';

export interface LogEntryDto {
  timestamp: string;
  level: number;
  message: string;
  messageTemplate: string;
  exception: string;
  properties: any;
  sourceContext: string;
}

@Injectable({ providedIn: 'root' })
export class LogsService {
  private baseUrl = `${environment.apiUrl}/logs`;
  private http = inject(HttpClient);

  getLogsPaginated(
    page: number,
    pageSize: number,
    from?: string,
    to?: string,
    level?: number
  ): Observable<PaginatedResult<LogEntryDto>> {
    let params = new HttpParams()
      .set('Page', String(page))
      .set('PageSize', String(pageSize));

    if (from) params = params.set('From', from);
    if (to) params = params.set('To', to);
    if (level) params = params.set('Level', level);

    return this.http.get<any>(this.baseUrl, { params, withCredentials: true }).pipe(
      map(res => ({
        ...res,
        items: res.value.map((x: any) => ({
          ...x,
          timestamp: new Date(x.timestamp)
        }))
      }))
    );
  }

  deleteOlderThan(cutoffDate: string): Observable<void> {
    const params = new HttpParams().set('cutoffDate', cutoffDate);
    return this.http.delete<void>(`${this.baseUrl}/older-than`, { params, withCredentials: true });
  }
}
