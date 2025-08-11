import {Component, DOCUMENT, inject, OnInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import { PagesApiService } from '../../../core/services/pages-api.service';
import {ActivatedRoute, NavigationEnd, Router, RouterOutlet} from '@angular/router';
import {map} from 'rxjs';
import {filter} from 'rxjs/operators';
import {ProjectDto} from '../../../shared/models/page-dtos';
import {TranslocoPipe} from '@ngneat/transloco';
import {SkillComponent} from '../../../shared/components/skills/skill.component/skill.component';

@Component({
  selector: 'app-portfolio',
  standalone: true,
  imports: [CommonModule, TranslocoPipe, RouterOutlet, SkillComponent],
  templateUrl: './portfolio.component.html',
  styleUrls: ['./portfolio.component.scss']
})
export class PortfolioComponent implements OnInit {
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  readonly page$ = inject(PagesApiService).portfolioPage$;
  private document = inject(DOCUMENT);

  readonly projects$ = this.page$.pipe(
    map(page =>
      page.projects.map(project => ({
        ...project,
        skills: [...project.skills].sort((a, b) => {
          const orderDiff = a.skill.category.displayOrder - b.skill.category.displayOrder;
          return orderDiff !== 0
            ? orderDiff
            : a.skill.name.localeCompare(b.skill.name);
        })
      }))
    )
  );

  hasDetail = false;

  expandedProjectId?: string;
  private hoverTimeout?: ReturnType<typeof setTimeout>;

  onMouseEnter(projectId: string) {
    this.hoverTimeout = setTimeout(() => {
      this.expandedProjectId = projectId;
    }, 1000);
  }

  onMouseLeave() {
    if (this.hoverTimeout) {
      clearTimeout(this.hoverTimeout);
      this.hoverTimeout = undefined;
    }
    this.expandedProjectId = undefined;
  }


  ngOnInit() {
    this.updateHasDetail();
    this.router.events.pipe(filter(e => e instanceof NavigationEnd)).subscribe(() => {
      this.updateHasDetail();
    });
  }

  private updateHasDetail() {
    const child = this.route.firstChild;
    this.hasDetail = !!(child && child.snapshot.paramMap.get('slug'));
  }

  trackBySlug(_: number, project: ProjectDto): string {
    return project.slug;
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
