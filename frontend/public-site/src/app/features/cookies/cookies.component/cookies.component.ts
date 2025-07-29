import {Component, inject} from '@angular/core';
import {PagesApiService} from '../../../core/services/pages-api.service';
import {AsyncPipe} from '@angular/common';

@Component({
  selector: 'app-cookies',
  standalone: true,
  templateUrl: './cookies.component.html',
  imports: [
    AsyncPipe
  ],
  styleUrl: './cookies.component.scss'
})
export class CookiesComponent {
  readonly page$ = inject(PagesApiService).cookiesPage$;
}
