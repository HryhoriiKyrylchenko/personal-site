import {Component, DOCUMENT, inject, OnInit} from '@angular/core';
import { PagesApiService } from '../../../core/services/pages-api.service';
import {AsyncPipe, NgStyle} from '@angular/common';
import {ActivatedRoute, NavigationEnd, Router, RouterOutlet} from '@angular/router';
import {TranslocoPipe} from '@ngneat/transloco';
import {map} from 'rxjs';
import {SharePopoverComponent} from '../share-popover.component/share-popover.component';
import {BlogPostDto} from '../../../shared/models/page-dtos';
import {filter} from 'rxjs/operators';
import {AnalyticsService} from '../../../core/services/analytics-service';

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

  hasDetail = false;

  private analytics = inject(AnalyticsService);

  ngOnInit() {
    this.updateHasDetail();
    this.router.events.pipe(filter(e => e instanceof NavigationEnd)).subscribe(() => {
      this.updateHasDetail();
    });

    this.analytics.trackEvent({
      eventType: "page_view",
      pageSlug: "blog",
      additionalDataJson: "{}"
    }).subscribe();
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
