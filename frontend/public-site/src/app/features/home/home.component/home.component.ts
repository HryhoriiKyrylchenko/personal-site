import {
  Component,
  computed,
  inject, OnInit
} from '@angular/core';
import { PagesApiService } from '../../../core/services/pages-api.service';
import {map, take} from 'rxjs';
import {AsyncPipe, NgStyle} from '@angular/common';
import {TranslocoPipe} from '@ngneat/transloco';
import {SkillComponent} from '../../../shared/components/skills/skill.component/skill.component';
import {Router} from '@angular/router';
import {SiteInfoService} from '../../../core/services/site-info.service';
import {MetaService} from '../../../core/services/meta.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    AsyncPipe,
    TranslocoPipe,
    SkillComponent,
    NgStyle
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {
  readonly page$ = inject(PagesApiService).homePage$;
  private router = inject(Router);
  private metaService = inject(MetaService);

  readonly skills$ = this.page$.pipe(
    map(page =>
      (page?.userSkills ?? []).slice().sort((a, b) => {
        const orderDiff = a.skill.category.displayOrder - b.skill.category.displayOrder;
        return orderDiff !== 0
          ? orderDiff
          : a.skill.name.localeCompare(b.skill.name);
      })
    )
  );

  readonly project$ = this.page$.pipe(
    map(page => page?.lastProject)
  );

  readonly projectSkills$ = this.project$.pipe(
    map(project =>
      (project?.skills ?? []).slice().sort((a, b) => {
        const orderDiff = a.skill.category.displayOrder - b.skill.category.displayOrder;
        return orderDiff !== 0
          ? orderDiff
          : a.skill.name.localeCompare(b.skill.name);
      })
    )
  );

  private svc = inject(SiteInfoService);
  resume = computed(() => this.svc.siteInfo()?.resume ?? null);

  socialLinks = computed(() => this.svc.siteInfo()?.socialLinks ?? []);
  private readonly sameAs = this.socialLinks()
    .filter(link => link.isActive)
    .map(link => link.url);

  ngOnInit() {
    this.page$.subscribe(page => {
      this.metaService.setSeo({
        title: page.pageData.metaTitle || 'Hryhorii Kyrylchenko',
        description: page.pageData.metaDescription || 'Personal portfolio page',
        imageUrl: page.pageData.ogImage || page.pageData.pageImage || undefined,
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

  onContactClick() {
    void this.router.navigate(['/contacts']);
  }

  onDownloadClick() {
    const resume = this.resume();
    if (resume?.fileUrl?.trim()) {
      const link = document.createElement('a');
      link.href = resume.fileUrl;
      link.download = resume.fileName || 'resume';
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
    }
  }

  onPortfolioClick() {
    void this.router.navigate(['/portfolio']);
  }

  onReadClick() {
    this.project$
      .pipe(take(1))
      .subscribe(project => {
        if (project?.slug) {
          void this.router.navigate(['/portfolio', project.slug]);
        }
      });
  }

  onRepoClick() {
    this.project$
      .pipe(take(1))
      .subscribe(project => {
        if (project?.repoUrl && project.repoUrl.trim()) {
          window.open(project.repoUrl, '_blank');
        }
      });
  }

  onDemoClick() {
    this.project$
      .pipe(take(1))
      .subscribe(project => {
        if (project?.demoUrl && project.demoUrl.trim()) {
          window.open(project.demoUrl, '_blank');
        }
      });
  }
}
