import {Component, Input, signal} from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterLink} from '@angular/router';
import {TranslocoPipe} from '@ngneat/transloco';
import {ThemeToggleComponent} from '../../../theme/theme-toggle.component/theme-toggle.component';
import {LanguageSwitcherComponent} from '../language-switcher.component/language-switcher.component';

@Component({
  selector: 'app-header-mobile',
  standalone: true,
  imports: [CommonModule, RouterLink, TranslocoPipe, ThemeToggleComponent, LanguageSwitcherComponent],
  templateUrl: './header-mobile.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderMobileComponent {
  @Input() navLinks: { textKey: string; path: string }[] = [];

  mobileNavOpen = signal(false);

  toggleMenu() {
    this.mobileNavOpen.set(!this.mobileNavOpen());
  }
}
