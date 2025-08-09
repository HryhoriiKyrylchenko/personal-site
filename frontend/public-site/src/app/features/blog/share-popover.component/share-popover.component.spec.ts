import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SharePopoverComponent } from './share-popover.component';

describe('SharePopoverComponent', () => {
  let component: SharePopoverComponent;
  let fixture: ComponentFixture<SharePopoverComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SharePopoverComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SharePopoverComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
