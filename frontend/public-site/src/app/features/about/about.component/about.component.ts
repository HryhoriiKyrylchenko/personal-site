import { Component, inject } from '@angular/core';
import { PagesApiService } from '../../../core/services/pages-api.service';
import {AsyncPipe, NgStyle} from '@angular/common';
import {TranslocoPipe} from '@ngneat/transloco';
import {SkillComponent} from '../../../shared/components/skills/skill.component/skill.component';
import {map} from 'rxjs';
import {UserSkillDto} from '../../../shared/models/page-dtos';
import {SkillLevelComponent} from '../skill-level.component/skill-level.component';
import {LearningSkillComponent} from '../learning-skill.component/learning-skill.component';

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
export class AboutComponent {
  readonly page$ = inject(PagesApiService).aboutPage$;

  readonly skills$ = this.page$.pipe(
    map(page => page?.userSkills ?? [])
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
    map(page =>
      (page?.learningSkills ?? []).slice().sort((a, b) => b.displayOrder - a.displayOrder)
    )
  );

  readonly contentParagraphs$ = this.page$.pipe(
    map(pageData =>
      (pageData?.pageData.data['Content'].split(/\n+/).map(p => p.trim()).filter(Boolean))
    )
  );
}
