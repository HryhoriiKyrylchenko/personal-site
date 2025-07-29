import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {filter, startWith, switchMap} from 'rxjs/operators';
import {CookiesPageDto, HomePageDto, PageDto, PrivacyPageDto} from '../../shared/models/page-dtos';
import { AboutPageDto } from '../../shared/models/page-dtos';
import { PortfolioPageDto } from '../../shared/models/page-dtos';
import { BlogPageDto } from '../../shared/models/page-dtos';
import { ContactPageDto } from '../../shared/models/page-dtos';
import {environment} from '../../../environments/environment';
import {TranslocoService} from '@ngneat/transloco';
import {EMPTY, of} from 'rxjs';

@Injectable({ providedIn: 'root' })
export class PagesApiService {
  private http = inject(HttpClient);
  private transloco = inject(TranslocoService);
  private baseUrl = `${environment.apiUrl}/pages`;

  private createPage$<T extends { pageData: PageDto }>(path: string) {
    return this.transloco.events$.pipe(
      filter(e => e.type === 'langChanged' || e.type === 'translationLoadSuccess'),
      startWith({  }),
      switchMap(() => {
        const initialLang = this.transloco.getActiveLang();
        if (!initialLang) return EMPTY;

        return this.http.get<T>(`${this.baseUrl}/${path}`).pipe(
          switchMap(res => {
            if(res.pageData.language && res.pageData.language != '') {
              const updatedLang = this.transloco.getActiveLang();
              if (res.pageData.language != updatedLang) {
                return this.http.get<T>(`${this.baseUrl}/${path}`);
              }
            }
            return of(res);
          })
        );
      })
    );
  }

  homePage$ = this.createPage$<HomePageDto>('home');
  aboutPage$ = this.createPage$<AboutPageDto>('about');
  blogPage$ = this.createPage$<BlogPageDto>('blog');
  portfolioPage$ = this.createPage$<PortfolioPageDto>('portfolio');
  contactsPage$ = this.createPage$<ContactPageDto>('contacts');
  privacyPage$ = this.createPage$<PrivacyPageDto>('privacy');
  cookiesPage$ = this.createPage$<CookiesPageDto>('cookies');
}
