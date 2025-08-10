import {Component, Input} from '@angular/core';

@Component({
  selector: 'app-skill-level',
  standalone: true,
  imports: [],
  templateUrl: './skill-level.component.html',
  styleUrl: './skill-level.component.scss'
})
export class SkillLevelComponent {
  @Input() level = 0;

  @Input() total = 5;

  get circles(): number[] {
    return Array(this.total).fill(0).map((_, i) => i + 1);
  }
}
