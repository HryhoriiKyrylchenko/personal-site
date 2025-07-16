import { Component } from '@angular/core';
import {TranslocoPipe} from '@ngneat/transloco';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-footer-links',
  imports: [
    TranslocoPipe,
    RouterLink
  ],
  templateUrl: './footer-links.component.html',
  styleUrl: './footer-links.component.scss'
})
export class FooterLinksComponent {

}
