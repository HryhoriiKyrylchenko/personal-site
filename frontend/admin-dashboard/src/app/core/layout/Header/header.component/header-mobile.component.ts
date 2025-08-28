import {Component, signal} from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-header-mobile',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './header-mobile.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderMobileComponent {
  mobileNavOpen = signal(false);

  toggleMenu() {
    this.mobileNavOpen.set(!this.mobileNavOpen());
  }
}
