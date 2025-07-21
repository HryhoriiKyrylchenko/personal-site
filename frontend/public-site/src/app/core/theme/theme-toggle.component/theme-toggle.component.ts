import { Component, inject, signal } from '@angular/core';
import { BreakpointObserver } from '@angular/cdk/layout';
import { toSignal } from '@angular/core/rxjs-interop';
import { ThemeService } from '../theme.service'
import { TranslocoPipe } from '@ngneat/transloco';
import {map} from 'rxjs';
import {CustomBreakpoints} from '../../../shared/utils/custom-breakpoints';

@Component({
  selector: 'app-theme-toggle',
  standalone: true,
  imports: [TranslocoPipe],
  templateUrl: './theme-toggle.component.html',
  styleUrls: ['./theme-toggle.component.scss']
})
export class ThemeToggleComponent {
  private themeService = inject(ThemeService);
  private bp = inject(BreakpointObserver);

  isDark = signal(false);

  isMobile = toSignal(
    this.bp.observe([CustomBreakpoints.Mobile])
      .pipe(map(res => res.matches)),
    { initialValue: false }
  );

  constructor() {
    this.isDark.set(this.themeService.getTheme() === 'dark');
  }

  toggleTheme() {
    this.themeService.toggleTheme();
    this.isDark.set(this.themeService.getTheme() === 'dark');
  }

  get labelKey() {
    return this.isDark() ? 'theme.light' : 'theme.dark';
  }
}
