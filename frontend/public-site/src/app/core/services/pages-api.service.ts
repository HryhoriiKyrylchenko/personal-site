import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {filter, startWith, switchMap} from 'rxjs/operators';
import {CookiesPageDto, HomePageDto, PrivacyPageDto} from '../../shared/models/page-dtos';
import { AboutPageDto } from '../../shared/models/page-dtos';
import { PortfolioPageDto } from '../../shared/models/page-dtos';
import { BlogPageDto } from '../../shared/models/page-dtos';
import { ContactPageDto } from '../../shared/models/page-dtos';
import {environment} from '../../../environments/environment';
import {toSignal} from '@angular/core/rxjs-interop';
import {TranslocoService} from '@ngneat/transloco';

@Injectable({ providedIn: 'root' })
export class PagesApiService {
  private http = inject(HttpClient);
  private transloco = inject(TranslocoService);
  private baseUrl = `${environment.apiUrl}/pages`;

  private createPageSignal<T>(path: string) {
    return toSignal(
      this.transloco.events$.pipe(
        filter(e => e.type === 'langChanged'),
        startWith({}), // стартовый триггер
        switchMap(() => this.http.get<T>(`${this.baseUrl}/${path}`))
      ),
      { initialValue: null as unknown as T }
    );
  }

  private homePageSignal = this.createPageSignal<HomePageDto>('home');
  private aboutPageSignal = this.createPageSignal<AboutPageDto>('about');
  private portfolioPageSignal = this.createPageSignal<PortfolioPageDto>('portfolio');
  private blogPageSignal = this.createPageSignal<BlogPageDto>('blog');
  private contactPageSignal = this.createPageSignal<ContactPageDto>('contacts');
  private cookiesPageSignal = this.createPageSignal<CookiesPageDto>('cookies');
  private privacyPageSignal = this.createPageSignal<PrivacyPageDto>('privacy');

  get homePage()     { return this.homePageSignal; }
  get aboutPage()    { return this.aboutPageSignal; }
  get portfolioPage(){ return this.portfolioPageSignal; }
  get blogPage()     { return this.blogPageSignal; }
  get contactPage()  { return this.contactPageSignal; }
  get cookiesPage()  { return this.cookiesPageSignal; }
  get privacyPage()  { return this.privacyPageSignal; }
}
