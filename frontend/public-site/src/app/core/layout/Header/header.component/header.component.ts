import {Component, Input} from '@angular/core';
import {RouterLink} from '@angular/router';
import {NgOptimizedImage} from '@angular/common';
import {ThemeToggleComponent} from '../../../theme/theme-toggle.component/theme-toggle.component';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [
    RouterLink,
    NgOptimizedImage,
    ThemeToggleComponent
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  @Input() logoSrc = '';
  @Input() navLinks: { text: string; path: string }[] = [];
}
