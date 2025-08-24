import {Component, computed, DOCUMENT, inject, OnInit} from '@angular/core';
import { PagesApiService } from '../../../core/services/pages-api.service';
import {AsyncPipe, NgStyle} from '@angular/common';
import {ActivatedRoute, NavigationEnd, Router, RouterOutlet} from '@angular/router';
import {TranslocoPipe} from '@ngneat/transloco';
import {map} from 'rxjs';
import {SharePopoverComponent} from '../share-popover.component/share-popover.component';
import {BlogPostDto} from '../../../shared/models/page-dtos';
import {filter} from 'rxjs/operators';
import {MetaService} from '../../../core/services/meta.service';
import {SiteInfoService} from '../../../core/services/site-info.service';

@Component({
  selector: 'app-blog',
  standalone: true,
  imports: [
    TranslocoPipe,
    AsyncPipe,
    NgStyle,
    SharePopoverComponent,
    RouterOutlet
  ],
  templateUrl: './blog.component.html',
  styleUrl: './blog.component.scss'
})
export class BlogComponent implements OnInit {
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  readonly page$ = inject(PagesApiService).blogPage$;
  readonly posts$ = this.page$.pipe(map(page => page.blogPosts));
  private document = inject(DOCUMENT);
  private metaService = inject(MetaService);

  private svc = inject(SiteInfoService);

  socialLinks = computed(() => this.svc.siteInfo()?.socialLinks ?? []);
  private readonly sameAs = this.socialLinks()
    .filter(link => link.isActive)
    .map(link => link.url);

  hasDetail = false;

  ngOnInit() {
    this.updateHasDetail();
    this.router.events.pipe(filter(e => e instanceof NavigationEnd)).subscribe(() => {
      this.updateHasDetail();
    });

    this.page$.subscribe(page => {
      this.metaService.setSeo({
        title: page.pageData.metaTitle || 'Hryhorii Kyrylchenko | Blog',
        description: page.pageData.metaDescription || 'Personal portfolio blog page',
        imageUrl: page.pageData.ogImage || undefined,
        url: window.location.href,
        type: 'profile',
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

  private updateHasDetail() {
    const child = this.route.firstChild;
    this.hasDetail = !!(child && child.snapshot.paramMap.get('slug'));
  }

  trackBySlug(_: number, post: BlogPostDto): string {
    return post.slug;
  }

  onReadClick(slug: string) {
    void this.router.navigate([slug], { relativeTo: this.route });
  }

  currentFullUrl(slug: string): string {
    const urlTree = this.router.createUrlTree([slug], { relativeTo: this.route });
    const path = this.router.serializeUrl(urlTree);
    return `${this.document.location.origin}${path}`;
  }
}
