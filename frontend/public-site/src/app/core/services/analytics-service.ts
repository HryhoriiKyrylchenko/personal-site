import {inject, Injectable} from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import {environment} from '../../../environments/environment';

export interface TrackAnalyticsEventRequest {
  eventType: string;
  pageSlug: string;
  additionalDataJson?: string;
}

@Injectable({
  providedIn: 'root'
})
export class AnalyticsService {
  private apiUrl = environment.analyticsApiUrl;
  private apiKey = environment.analyticsApiKey;

  private http = inject(HttpClient);

  trackEvent(request: TrackAnalyticsEventRequest) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'X-Api-Key': this.apiKey
    });

    return this.http.post(this.apiUrl, request, { headers });
  }
}
