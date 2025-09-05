import {Component, inject, Input, signal} from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterLink} from '@angular/router';
import {ThemeToggleComponent} from '../../../theme/theme-toggle.component/theme-toggle.component';

@Component({
  selector: 'app-header-tablet',
  standalone: true,
  imports: [CommonModule, RouterLink, ThemeToggleComponent],
  templateUrl: './header-tablet.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderTabletComponent {
  @Input() navLinks: { text: string; path: string }[] = [];

  mobileNavOpen = signal(false);

  toggleMenu() {
    this.mobileNavOpen.set(!this.mobileNavOpen());
  }
}
