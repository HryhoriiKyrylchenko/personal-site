import {Component, Input} from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterLink} from '@angular/router';
import {TranslocoPipe} from '@ngneat/transloco';
import {ThemeToggleComponent} from '../../../theme/theme-toggle.component/theme-toggle.component';
import {LanguageSwitcherComponent} from '../language-switcher.component/language-switcher.component';

@Component({
  selector: 'app-header-desktop',
  standalone: true,
  imports: [CommonModule, RouterLink, TranslocoPipe, ThemeToggleComponent, LanguageSwitcherComponent],
  templateUrl: './header-desktop.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderDesktopComponent {
  @Input() navLinks: { textKey: string; path: string }[] = [];
}
