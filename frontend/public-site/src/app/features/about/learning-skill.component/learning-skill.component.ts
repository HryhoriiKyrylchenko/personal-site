import {Component, Input} from '@angular/core';

export enum LearningStatus {
  Planned = 'Planned',
  InProgress = 'InProgress',
  Practicing = 'Practicing'
}

@Component({
  selector: 'app-learning-skill',
  standalone: true,
  imports: [],
  templateUrl: './learning-skill.component.html',
  styleUrl: './learning-skill.component.scss'
})
export class LearningSkillComponent {
  @Input() name = '';

  @Input() status!: string;

  @Input() description = '';

  LearningStatus = LearningStatus;

  private statusConfig: Record<LearningStatus, { text: string; color: string }> = {
    [LearningStatus.Planned]: { text: 'Planned',     color: '#FF9169' },
    [LearningStatus.InProgress]: { text: 'In Progress', color: '#41ECFF' },
    [LearningStatus.Practicing]: { text: 'Practicing',  color: '#76FF9F' }
  };

  get statusText(): string {
    return this.statusConfig[this.status as LearningStatus]?.text ?? this.status;
  }

  get statusColor(): string {
    return this.statusConfig[this.status as LearningStatus]?.color ?? '#999';
  }
}
