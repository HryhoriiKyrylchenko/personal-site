import {Component, signal} from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-header-tablet',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './header-tablet.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderTabletComponent {
  mobileNavOpen = signal(false);

  toggleMenu() {
    this.mobileNavOpen.set(!this.mobileNavOpen());
  }
}
