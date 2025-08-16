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
import {AnalyticsService} from '../../../core/services/analytics-service';

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

  private analytics = inject(AnalyticsService);

  ngOnInit() {
    this.analytics.trackEvent({
      eventType: "page_view",
      pageSlug: "home",
      additionalDataJson: "{}"
    }).subscribe();
  }

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
