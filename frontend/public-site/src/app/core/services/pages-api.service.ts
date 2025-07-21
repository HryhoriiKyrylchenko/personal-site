import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { take } from 'rxjs/operators';
import { Observable } from 'rxjs';
import {CookiesPageDto, HomePageDto, PrivacyPageDto} from '../../shared/models/page-dtos';
import { AboutPageDto } from '../../shared/models/page-dtos';
import { PortfolioPageDto } from '../../shared/models/page-dtos';
import { BlogPageDto } from '../../shared/models/page-dtos';
import { ContactPageDto } from '../../shared/models/page-dtos';
import {environment} from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class PagesApiService {
  private http = inject(HttpClient);
  private baseUrl = `${environment.apiUrl}/pages`;

  getHomePage(): Observable<HomePageDto> {
    return this.http.get<HomePageDto>(`${this.baseUrl}/home`).pipe(take(1));
  }

  getAboutPage(): Observable<AboutPageDto> {
    return this.http.get<AboutPageDto>(`${this.baseUrl}/about`).pipe(take(1));
  }

  getPortfolioPage(): Observable<PortfolioPageDto> {
    return this.http.get<PortfolioPageDto>(`${this.baseUrl}/portfolio`).pipe(take(1));
  }

  getBlogPage(): Observable<BlogPageDto> {
    return this.http.get<BlogPageDto>(`${this.baseUrl}/blog`).pipe(take(1));
  }

  getContactPage(): Observable<ContactPageDto> {
    return this.http.get<ContactPageDto>(`${this.baseUrl}/contacts`).pipe(take(1));
  }

  getCookiesPage(): Observable<CookiesPageDto> {
    return this.http.get<CookiesPageDto>(`${this.baseUrl}/cookies`).pipe(take(1));
  }

  getPrivacyPage(): Observable<PrivacyPageDto> {
    return this.http.get<PrivacyPageDto>(`${this.baseUrl}/privacy`).pipe(take(1));
  }
}
