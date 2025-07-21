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
  private cookieName = 'cookie_consent';
  private cookieService = inject(CookieService);

  ngOnInit() {
    if (typeof window !== 'undefined') {
      Promise.resolve().then(() => {
        this.show = !this.cookieService.check(this.cookieName);
      });
    }
  }

  accept() {
    this.cookieService.set(this.cookieName, 'yes', 365, '/');
    this.show = false;
  }

  learnMore() {
    this.accept();
  }
}
