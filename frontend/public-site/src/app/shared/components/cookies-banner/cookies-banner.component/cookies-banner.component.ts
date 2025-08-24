import {Component, inject, OnInit} from '@angular/core';
import {CookieService} from 'ngx-cookie-service';
import {RouterLink} from '@angular/router';
import {CommonModule} from '@angular/common';
import {TranslocoPipe} from '@ngneat/transloco';

@Component({
  selector: 'app-cookies-banner',
  standalone: true,
  providers: [CookieService],
  imports: [
    CommonModule,
    RouterLink,
    TranslocoPipe
  ],
  templateUrl: './cookies-banner.component.html',
  styleUrl: './cookies-banner.component.scss'
})
export class CookiesBannerComponent implements OnInit {
  show = false;
  private readonly cookieName = 'cookie_consent';
  private readonly cookieService = inject(CookieService);

  ngOnInit(): void {
    if (typeof window !== 'undefined') {
      Promise.resolve().then(() => {
        this.show = !this.cookieService.check(this.cookieName);
      });
    }
  }

  accept(): void {
    this.cookieService.set(this.cookieName, 'yes', 365, '/');
    this.show = false;
  }

  learnMore(): void {
    this.accept();
  }
}
