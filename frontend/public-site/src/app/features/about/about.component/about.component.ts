import {
  Component, computed,
  inject, OnInit
} from '@angular/core';
import { PagesApiService } from '../../../core/services/pages-api.service';
import {AsyncPipe, NgStyle} from '@angular/common';
import {TranslocoPipe} from '@ngneat/transloco';
import {SkillComponent} from '../../../shared/components/skills/skill.component/skill.component';
import {map} from 'rxjs';
import {UserSkillDto} from '../../../shared/models/page-dtos';
import {SkillLevelComponent} from '../skill-level.component/skill-level.component';
import {LearningSkillComponent} from '../learning-skill.component/learning-skill.component';
import {MetaService} from '../../../core/services/meta.service';
import {SiteInfoService} from '../../../core/services/site-info.service';

@Component({
  selector: 'app-about',
  standalone: true,
  imports: [
    AsyncPipe,
    NgStyle,
    TranslocoPipe,
    SkillComponent,
    SkillLevelComponent,
    LearningSkillComponent
  ],
  templateUrl: './about.component.html',
  styleUrl: './about.component.scss'
})
export class AboutComponent implements OnInit {
  readonly page$ = inject(PagesApiService).aboutPage$;
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

  readonly skillsByCategory$ = this.skills$.pipe(
    map(skills => {
      const grouped: Record<string, UserSkillDto[]> = {
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

  readonly learningSkills$ = this.page$.pipe(
    map(page => page?.learningSkills ?? [])
  );

  readonly contentParagraphs$ = this.page$.pipe(
    map(pageData =>
      (pageData?.pageData.data['Content'].split(/\n+/).map(p => p.trim()).filter(Boolean))
    )
  );

  private svc = inject(SiteInfoService);

  socialLinks = computed(() => this.svc.siteInfo()?.socialLinks ?? []);
  private readonly sameAs = this.socialLinks()
    .filter(link => link.isActive)
    .map(link => link.url);

  ngOnInit() {
    this.page$.subscribe(page => {
      this.metaService.setSeo({
        title: page.pageData.metaTitle || 'Hryhorii Kyrylchenko | About',
        description: page.pageData.metaDescription || 'Personal portfolio about page',
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
}
