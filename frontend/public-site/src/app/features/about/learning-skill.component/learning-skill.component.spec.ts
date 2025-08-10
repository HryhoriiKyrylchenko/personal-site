import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LearningSkillComponent } from './learning-skill.component';

describe('LearningSkillComponent', () => {
  let component: LearningSkillComponent;
  let fixture: ComponentFixture<LearningSkillComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LearningSkillComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LearningSkillComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
