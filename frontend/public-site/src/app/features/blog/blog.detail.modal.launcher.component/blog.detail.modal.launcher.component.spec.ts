import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BlogDetaitModalLauncherComponent } from './blog.detail.modal.launcher.component';

describe('BlogDetaitModalLauncherComponent', () => {
  let component: BlogDetaitModalLauncherComponent;
  let fixture: ComponentFixture<BlogDetaitModalLauncherComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BlogDetaitModalLauncherComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BlogDetaitModalLauncherComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
