import { Component, inject } from '@angular/core';
import { PagesApiService } from '../../../core/services/pages-api.service';
import { HomePageDto } from '../../../shared/models/page-dtos';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {
  private pagesApi = inject(PagesApiService);
  data?: HomePageDto;

  constructor() {
    //this.pagesApi.getHomePage().subscribe(dto => { this.data = dto; });
  }
}
