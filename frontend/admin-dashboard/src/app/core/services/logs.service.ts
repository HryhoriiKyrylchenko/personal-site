import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {map, Observable} from 'rxjs';
import {environment} from '../../../environments/environment';

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
  private baseUrl = `${environment.apiUrl}/Logs`;

  constructor(private http: HttpClient) {}

  getRecentLogs(count: number): Observable<LogEntryDto[]> {
    return this.http.get<{ value: LogEntryDto[] }>(this.baseUrl, { params: { count: count.toString() } })
      .pipe(
        map(res => res.value)
      );
  }

  deleteOlderThan(cutoff: string): Observable<{ deleted: number }> {
    return this.http.delete<{ deleted: number }>(this.baseUrl, { params: { cutoff } });
  }
}
