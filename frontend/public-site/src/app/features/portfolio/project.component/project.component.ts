import {Component, computed, DOCUMENT, inject, OnInit, PLATFORM_ID} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {PagesApiService} from '../../../core/services/pages-api.service';
import {Meta, Title} from '@angular/platform-browser';
import {switchMap, tap} from 'rxjs/operators';
import {map, of} from 'rxjs';
import {AsyncPipe, isPlatformBrowser, NgStyle} from '@angular/common';
import {MatButton} from '@angular/material/button';
import {ShareLinkComponent} from '../../blog/share-link.component/share-link.component';
import {TranslocoPipe} from '@ngneat/transloco';
import {ProjectSkillDto} from '../../../shared/models/page-dtos';
import {SkillComponent} from '../../../shared/components/skills/skill.component/skill.component';
import {MetaService} from '../../../core/services/meta.service';
import {SiteInfoService} from '../../../core/services/site-info.service';

@Component({
  selector: 'app-project',
  standalone: true,
  imports: [
    AsyncPipe,
    MatButton,
    ShareLinkComponent,
    TranslocoPipe,
    NgStyle,
    SkillComponent
  ],
  templateUrl: './project.component.html',
  styleUrl: './project.component.scss'
})
export class ProjectComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private api = inject(PagesApiService);
  private meta = inject(Meta);
  private titleService = inject(Title);
  private platformId = inject(PLATFORM_ID);
  private document = inject(DOCUMENT);

  private metaService = inject(MetaService);

  private svc = inject(SiteInfoService);

  socialLinks = computed(() => this.svc.siteInfo()?.socialLinks ?? []);
  private readonly sameAs = this.socialLinks()
    .filter(link => link.isActive)
    .map(link => link.url);

  readonly project$ = this.route.paramMap.pipe(
    switchMap(params => {
      const slug = params.get('slug');
      if (!slug) return of(null);

      return this.api.portfolioPage$.pipe(
        map(page => page.projects.find(p => p.slug === slug) ?? null),
        tap(project => {
          if (project) {
            this.titleService.setTitle(project.metaTitle);
            this.meta.updateTag({ name: 'description', content: project.metaDescription });
            this.meta.updateTag({ property: 'og:image', content: project.ogImage });
          }
        })
      );
    })
  );

  readonly skills$ = this.project$.pipe(
    map(project =>
      (project?.skills ?? []).slice().sort((a, b) => {
        const orderDiff = a.skill.category.displayOrder - b.skill.category.displayOrder;
        return orderDiff !== 0
          ? orderDiff
          : a.skill.name.localeCompare(b.skill.name);
      })
    )
  );

  readonly skillsByCategory$ = this.skills$.pipe(
    map(skills => {
      const grouped: Record<string, ProjectSkillDto[]> = {
        Languages: [],
        Tools: [],
        Frameworks: []
      };

      for (const skill of skills) {
        const key = skill.skill.category.key;
        if (grouped[key]) {
          grouped[key].push(skill);
        }
      }

      return grouped;
    })
  );

  ngOnInit() {
    this.project$.subscribe(project => {
      this.metaService.setSeo({
        title: project?.metaTitle || 'Hryhorii Kyrylchenko | Project',
        description: project?.metaDescription || 'Personal portfolio project page',
        imageUrl: project?.ogImage || project?.coverImage || undefined,
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
