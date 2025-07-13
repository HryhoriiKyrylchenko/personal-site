import { Component, inject } from '@angular/core';
import { PagesApiService } from '../../../core/services/pages-api.service';
import { BlogPageDto } from '../../../shared/models/page-dtos';

@Component({
  selector: 'app-blog',
  standalone: true,
  imports: [],
  templateUrl: './blog.component.html',
  styleUrl: './blog.component.scss'
})
export class BlogComponent {
  private pagesApi = inject(PagesApiService);
  data?: BlogPageDto;

  constructor() {
    //this.pagesApi.getBlogPage().subscribe(dto => (this.data = dto));
  }
}
