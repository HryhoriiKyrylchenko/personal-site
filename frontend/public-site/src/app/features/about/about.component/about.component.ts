import { Component, inject } from '@angular/core';
import { PagesApiService } from '../../../core/services/pages-api.service';
import { AboutPageDto } from '../../../shared/models/page-dtos';

@Component({
  selector: 'app-about',
  standalone: true,
  imports: [],
  templateUrl: './about.component.html',
  styleUrl: './about.component.scss'
})
export class AboutComponent {
  private pagesApi = inject(PagesApiService);
  data?: AboutPageDto;

  constructor() {
    //this.pagesApi.getAboutPage().subscribe(dto => { this.data = dto; });
  }
}
