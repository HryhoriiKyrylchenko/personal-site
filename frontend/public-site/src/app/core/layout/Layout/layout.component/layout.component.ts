import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from '../../Header/header.component/header.component';
import { FooterComponent } from '../../Footer/footer.component/footer.component';


@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent, FooterComponent],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss'
})
export class LayoutComponent {
  logo = '../logo.svg';
  links = [
    { text: 'Home', path: '/' },
    { text: 'About', path: '/about' },
    { text: 'Portfolio', path: '/portfolio' },
    { text: 'Blog', path: '/blog' },
    { text: 'Contacts', path: '/contacts' },
  ];
}
