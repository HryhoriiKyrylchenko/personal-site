import {Component, computed, DOCUMENT, inject, OnInit, PLATFORM_ID} from '@angular/core';
import {Meta, Title} from '@angular/platform-browser';
import {ActivatedRoute, Router} from '@angular/router';
import {switchMap, tap} from 'rxjs/operators';
import {PagesApiService} from '../../../core/services/pages-api.service';
import {MatButton} from '@angular/material/button';
import {AsyncPipe, isPlatformBrowser, NgStyle} from '@angular/common';
import {ShareLinkComponent} from '../share-link.component/share-link.component';
import {map, of} from 'rxjs';
import {TranslocoPipe} from '@ngneat/transloco';
import {MetaService} from '../../../core/services/meta.service';
import {SiteInfoService} from '../../../core/services/site-info.service';

@Component({
  selector: 'app-blog-post',
  standalone: true,
  imports: [
    MatButton,
    ShareLinkComponent,
    AsyncPipe,
    NgStyle,
    TranslocoPipe
  ],
  templateUrl: './blog-post.component.html',
  styleUrl: './blog-post.component.scss'
})
export class BlogPostComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private api = inject(PagesApiService);
  private meta = inject(Meta);
  private titleService = inject(Title);
  private platformId = inject(PLATFORM_ID);
  private document = inject(DOCUMENT);

  readonly post$ = this.route.paramMap.pipe(
    switchMap(params => {
      const slug = params.get('slug');
      if (!slug) return of(null);

      return this.api.blogPage$.pipe(
        map(page => page.blogPosts.find(p => p.slug === slug) ?? null),
        tap(post => {
          if (post) {
            this.titleService.setTitle(post.metaTitle);
            this.meta.updateTag({ name: 'description', content: post.metaDescription });
            this.meta.updateTag({ property: 'og:image', content: post.ogImage });
          }
        })
      );
    })
  );

  private metaService = inject(MetaService);

  private svc = inject(SiteInfoService);

  socialLinks = computed(() => this.svc.siteInfo()?.socialLinks ?? []);
  private readonly sameAs = this.socialLinks()
    .filter(link => link.isActive)
    .map(link => link.url);

  ngOnInit() {
    this.post$.subscribe(post => {
      this.metaService.setSeo({
        title: post?.metaTitle || 'Hryhorii Kyrylchenko | Blog Post',
        description: post?.metaDescription || 'Personal portfolio post page',
        imageUrl: post?.ogImage || post?.coverImage || undefined,
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

  currentUrl(): string {
    if (isPlatformBrowser(this.platformId)) {
      return this.document.location.href;
    } else {
      return '';
    }
  }

  iconSize = '3.0075rem';

  onCloseClick(): void {
    void this.router.navigate(['../'], { relativeTo: this.route });
  }
}
