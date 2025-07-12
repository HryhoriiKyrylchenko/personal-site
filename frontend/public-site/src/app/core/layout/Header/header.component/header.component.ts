import {Component, Input} from '@angular/core';
import {RouterLink} from '@angular/router';
import {ThemeToggleComponent} from '../../../theme/theme-toggle.component/theme-toggle.component';
import {LanguageSwitcherComponent} from '../language-switcher.component/language-switcher.component';
import {TranslocoPipe} from '@ngneat/transloco';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [
    RouterLink,
    ThemeToggleComponent,
    LanguageSwitcherComponent,
    TranslocoPipe
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  @Input() logoSrc = '';
  @Input() navLinks: { textKey: string; path: string }[] = [];
}
