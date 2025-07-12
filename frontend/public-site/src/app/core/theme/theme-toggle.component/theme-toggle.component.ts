import { Component, inject, signal } from '@angular/core';
import { ThemeService } from '../theme.service'
import { TranslocoPipe } from '@ngneat/transloco';

@Component({
  selector: 'app-theme-toggle',
  standalone: true,
  imports: [TranslocoPipe],
  templateUrl: './theme-toggle.component.html',
  styleUrls: ['./theme-toggle.component.scss']
})
export class ThemeToggleComponent {
  private themeService = inject(ThemeService);
  isDark = signal(false);

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
