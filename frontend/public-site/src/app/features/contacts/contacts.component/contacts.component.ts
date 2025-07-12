import { Component } from '@angular/core';
import { inject } from '@angular/core';
import { PagesApiService } from '../../../core/services/pages-api.service';
import { ContactPageDto } from '../../../shared/models/page-dtos';

@Component({
  selector: 'app-contacts',
  templateUrl: './contacts.component.html',
  styleUrl: './contacts.component.scss',
  standalone: true,
  imports: []
})
export class ContactsComponent {
  private pagesApi = inject(PagesApiService);
  data?: ContactPageDto;

  constructor() {
    //this.pagesApi.getContactPage().subscribe(dto => (this.data = dto));
  }
}
