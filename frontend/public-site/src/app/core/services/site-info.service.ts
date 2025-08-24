import {Injectable, inject, signal} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SiteInfoDto } from '../../shared/models/site-info-dtos';
import {environment} from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class SiteInfoService {
  private readonly http = inject(HttpClient);
  private baseUrl = `${environment.apiUrl}/site-info`;
  private _siteInfo = signal<SiteInfoDto | null>(null);
  readonly siteInfo = this._siteInfo.asReadonly();

  loadSiteInfo() {
    this.http.get<SiteInfoDto>(`${this.baseUrl}`).subscribe({
      next: data => this._siteInfo.set(data),
      error: err => console.error('SiteInfo load failed', err)
    });
  }
}
