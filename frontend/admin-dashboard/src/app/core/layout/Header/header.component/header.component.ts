import {Component, inject} from '@angular/core';
import { BreakpointObserver } from '@angular/cdk/layout';
import { CommonModule } from '@angular/common';
import { HeaderMobileComponent } from './header-mobile.component';
import { HeaderTabletComponent } from './header-tablet.component';
import { HeaderDesktopComponent } from './header-desktop.component';
import {map} from 'rxjs';
import {CustomBreakpoints} from '../../../../shared/utils/custom-breakpoints';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [
    CommonModule,
    HeaderMobileComponent,
    HeaderTabletComponent,
    HeaderDesktopComponent,
    HeaderTabletComponent
  ],
  template: `
    @switch (screenSize$ | async) {
      @case ('mobile') {
        <app-header-mobile [navLinks]="navLinks"></app-header-mobile>
      }
      @case ('tablet') {
        <app-header-tablet [navLinks]="navLinks"></app-header-tablet>
      }
      @case ('desktop') {
        <app-header-desktop [navLinks]="navLinks"></app-header-desktop>
      }
    }
  `
})
export class HeaderComponent {
  public navLinks: { text: string; path: string }[] = [
    { text: 'Analytics', path: '/' },
    { text: 'Blog', path: '/blog' },
    { text: 'Portfolio', path: '/portfolio' },
    { text: 'General', path: '/general' },
  ];

  private bp = inject(BreakpointObserver);

  screenSize$ = this.bp.observe([
    CustomBreakpoints.Mobile,
    CustomBreakpoints.Tablet,
    CustomBreakpoints.Desktop
  ]).pipe(
    map(state => {
      if (state.breakpoints[CustomBreakpoints.Mobile]) return 'mobile';
      if (state.breakpoints[CustomBreakpoints.Tablet]) return 'tablet';
      if (state.breakpoints[CustomBreakpoints.Desktop]) return 'desktop';
      return 'desktop';
    })
  );
}
