import {Component, inject, Input, signal} from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterLink} from '@angular/router';
import {ThemeToggleComponent} from '../../../theme/theme-toggle.component/theme-toggle.component';
import {AccountButtonsComponent} from '../account-buttons.component/account-buttons.component';

@Component({
  selector: 'app-header-mobile',
  standalone: true,
  imports: [CommonModule, RouterLink, ThemeToggleComponent, AccountButtonsComponent],
  templateUrl: './header-mobile.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderMobileComponent {
  @Input() navLinks: { text: string; path: string }[] = [];

  mobileNavOpen = signal(false);

  toggleMenu() {
    this.mobileNavOpen.set(!this.mobileNavOpen());
  }
}
