import {Component, inject, Input} from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterLink} from '@angular/router';
import {ThemeToggleComponent} from '../../../theme/theme-toggle.component/theme-toggle.component';


@Component({
  selector: 'app-header-desktop',
  standalone: true,
  imports: [CommonModule, RouterLink, ThemeToggleComponent],
  templateUrl: './header-desktop.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderDesktopComponent {
  @Input() navLinks: { text: string; path: string }[] = [];
}
