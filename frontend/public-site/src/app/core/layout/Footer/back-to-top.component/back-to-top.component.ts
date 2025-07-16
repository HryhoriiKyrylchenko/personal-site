import { Component, HostListener } from '@angular/core';
import {TranslocoPipe} from '@ngneat/transloco';

@Component({
  selector: 'app-back-to-top',
  imports: [
    TranslocoPipe
  ],
  templateUrl: './back-to-top.component.html',
  styleUrl: './back-to-top.component.scss'
})
export class BackToTopComponent {
  visible = false;

  @HostListener('window:scroll')
  onWindowScroll() {
    const scrollPos = (window.pageYOffset || document.documentElement.scrollTop) + window.innerHeight;
    const docHeight = document.documentElement.scrollHeight;
    this.visible = scrollPos >= docHeight;
  }

  scrollToTop() {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }
}
