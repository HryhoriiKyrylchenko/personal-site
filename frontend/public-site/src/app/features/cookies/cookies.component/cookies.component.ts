import {Component, computed, inject, OnInit} from '@angular/core';
import {PagesApiService} from '../../../core/services/pages-api.service';
import {AsyncPipe} from '@angular/common';
import {MetaService} from '../../../core/services/meta.service';
import {SiteInfoService} from '../../../core/services/site-info.service';

@Component({
  selector: 'app-cookies',
  standalone: true,
  templateUrl: './cookies.component.html',
  imports: [
    AsyncPipe
  ],
  styleUrl: './cookies.component.scss'
})
export class CookiesComponent implements OnInit {
  readonly page$ = inject(PagesApiService).cookiesPage$;
  private metaService = inject(MetaService);
  private svc = inject(SiteInfoService);

  socialLinks = computed(() => this.svc.siteInfo()?.socialLinks ?? []);
  private readonly sameAs = this.socialLinks()
    .filter(link => link.isActive)
    .map(link => link.url);

  ngOnInit() {
    this.page$.subscribe(page => {
      this.metaService.setSeo({
        title: page.pageData.metaTitle || 'Cookies Policy | Hryhorii Kyrylchenko',
        description: page.pageData.metaDescription || 'Personal portfolio cookies page',
        imageUrl: page.pageData.ogImage || undefined,
        url: window.location.href,
        type: 'article',
        jsonLd: {
          "@context": "https://schema.org",
          "@type": "Person",
          "name": "Hryhorii Kyrylchenko",
          "url": window.location.href,
          "sameAs": this.sameAs
        }
      });
    });
  }
}
