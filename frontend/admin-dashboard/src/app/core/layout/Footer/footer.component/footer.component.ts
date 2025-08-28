import {Component} from '@angular/core';
import {BackToTopComponent} from '../back-to-top.component/back-to-top.component';

@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [
    BackToTopComponent
  ],
  templateUrl: './footer.component.html',
  styleUrl: './footer.component.scss'
})
export class FooterComponent {
  currentYear = new Date().getFullYear();
}
