import { Component, inject, signal } from '@angular/core';
import { ThemeService } from '../theme.service'

@Component({
  selector: 'app-theme-toggle',
  standalone: true,
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
}
