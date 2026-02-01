import {Component, inject, Input} from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterLink} from '@angular/router';
import {ThemeToggleComponent} from '../../../theme/theme-toggle.component/theme-toggle.component';
import {AccountButtonsComponent} from '../account-buttons.component/account-buttons.component';


@Component({
  selector: 'app-header-desktop',
  standalone: true,
  imports: [CommonModule, RouterLink, ThemeToggleComponent, AccountButtonsComponent],
  templateUrl: './header-desktop.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderDesktopComponent {
  @Input() navLinks: { text: string; path: string }[] = [];
}
