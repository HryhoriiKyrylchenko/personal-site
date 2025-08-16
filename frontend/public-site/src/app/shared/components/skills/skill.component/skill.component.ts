import {Component, Input} from '@angular/core';
import {SkillDto} from '../../../models/page-dtos';

@Component({
  selector: 'app-skill',
  standalone: true,
  templateUrl: './skill.component.html',
  styleUrl: './skill.component.scss'
})
export class SkillComponent {
  @Input() skill?: SkillDto;
}
