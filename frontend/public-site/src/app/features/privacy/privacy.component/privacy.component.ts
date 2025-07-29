import {Component, inject} from '@angular/core';
import {PagesApiService} from '../../../core/services/pages-api.service';
import {AsyncPipe} from '@angular/common';

@Component({
  selector: 'app-privacy',
  standalone: true,
  templateUrl: './privacy.component.html',
  imports: [
    AsyncPipe
  ],
  styleUrl: './privacy.component.scss'
})
export class PrivacyComponent {
  readonly page$ = inject(PagesApiService).privacyPage$;
}
