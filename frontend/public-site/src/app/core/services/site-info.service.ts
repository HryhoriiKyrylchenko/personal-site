import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SiteInfoDto, SocialMediaLinkDto } from '../../shared/models/site-info-dtos';
import { Observable, map, shareReplay } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class SiteInfoService {
  private readonly http = inject(HttpClient);
  private siteInfo$ = this.http
    .get<SiteInfoDto>('/api/site-info')
    .pipe(shareReplay(1));

  private readonly allowedPlatforms = ['LinkedIn', 'Facebook', 'GitHub'];

  get socialMediaLinks$(): Observable<SocialMediaLinkDto[]> {
    return this.siteInfo$.pipe(
      map(info =>
        info.socialLinks
          .filter(link =>
            link.isActive && this.allowedPlatforms.includes(link.platform)
          )
          .sort((a, b) => a.displayOrder - b.displayOrder)
      )
    );
  }
}
