import {Component, computed, inject, OnInit} from '@angular/core';
import {PagesApiService} from '../../../core/services/pages-api.service';
import {AsyncPipe} from '@angular/common';
import {MetaService} from '../../../core/services/meta.service';
import {SiteInfoService} from '../../../core/services/site-info.service';

@Component({
  selector: 'app-privacy',
  standalone: true,
  templateUrl: './privacy.component.html',
  imports: [
    AsyncPipe
  ],
  styleUrl: './privacy.component.scss'
})
export class PrivacyComponent implements OnInit {
  readonly page$ = inject(PagesApiService).privacyPage$;
  private metaService = inject(MetaService);
  private svc = inject(SiteInfoService);

  socialLinks = computed(() => this.svc.siteInfo()?.socialLinks ?? []);
  private readonly sameAs = this.socialLinks()
    .filter(link => link.isActive)
    .map(link => link.url);

  ngOnInit() {
    this.page$.subscribe(page => {
      this.metaService.setSeo({
        title: page.pageData.metaTitle || 'Privacy Policy | Hryhorii Kyrylchenko',
        description: page.pageData.metaDescription || 'Personal portfolio privacy page',
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
