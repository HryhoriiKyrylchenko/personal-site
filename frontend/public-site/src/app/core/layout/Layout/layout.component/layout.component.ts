import {Component, inject, PLATFORM_ID, AfterViewInit} from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {CommonModule, isPlatformBrowser} from '@angular/common';
import { HeaderComponent } from '../../Header/header.component/header.component';
import { FooterComponent } from '../../Footer/footer.component/footer.component';
import {TranslocoService} from '@ngneat/transloco';
import {first} from 'rxjs';


@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent, FooterComponent, HeaderComponent, CommonModule],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss'
})
export class LayoutComponent implements AfterViewInit {
  private transloco = inject(TranslocoService);
  private platformId = inject(PLATFORM_ID);

  ngAfterViewInit() {
    if (isPlatformBrowser(this.platformId)) {
      this.transloco.langChanges$.pipe(first()).subscribe(() => {
        document.querySelector('.app-root')?.classList.remove('lang-loading');
      });
    }
  }

  links = [
    { textKey: 'nav.home', path: '/' },
    { textKey: 'nav.about', path: '/about' },
    { textKey: 'nav.portfolio', path: '/portfolio' },
    { textKey: 'nav.blog', path: '/blog' },
    { textKey: 'nav.contacts', path: '/contacts' },
  ];
}
