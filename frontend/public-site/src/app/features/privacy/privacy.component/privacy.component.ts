import {Component, inject} from '@angular/core';
import {PagesApiService} from '../../../core/services/pages-api.service';

@Component({
  selector: 'app-privacy',
  standalone: true,
  imports: [],
  templateUrl: './privacy.component.html',
  styleUrl: './privacy.component.scss'
})
export class PrivacyComponent {
  readonly page = inject(PagesApiService).privacyPage;
}
