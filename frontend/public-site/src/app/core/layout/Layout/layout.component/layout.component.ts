import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from '../../Header/header.component/header.component';
import { FooterComponent } from '../../Footer/footer.component/footer.component';


@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent, FooterComponent, HeaderComponent],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss'
})
export class LayoutComponent {
  links = [
    { textKey: 'nav.home', path: '/' },
    { textKey: 'nav.about', path: '/about' },
    { textKey: 'nav.portfolio', path: '/portfolio' },
    { textKey: 'nav.blog', path: '/blog' },
    { textKey: 'nav.contacts', path: '/contacts' },
  ];
}
