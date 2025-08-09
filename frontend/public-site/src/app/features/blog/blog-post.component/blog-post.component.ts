import {Component, DOCUMENT, inject, PLATFORM_ID} from '@angular/core';
import {Meta, Title} from '@angular/platform-browser';
import {ActivatedRoute, Router} from '@angular/router';
import {switchMap, tap} from 'rxjs/operators';
import {PagesApiService} from '../../../core/services/pages-api.service';
import {MatButton} from '@angular/material/button';
import {AsyncPipe, isPlatformBrowser, NgStyle} from '@angular/common';
import {ShareLinkComponent} from '../share-link.component/share-link.component';
import {map, of} from 'rxjs';
import {TranslocoPipe} from '@ngneat/transloco';

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
export class BlogPostComponent {
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
