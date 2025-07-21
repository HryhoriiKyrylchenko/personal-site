import {Component, inject, OnInit} from '@angular/core';
import {PageDto} from '../../../shared/models/page-dtos';
import {PagesApiService} from '../../../core/services/pages-api.service';

@Component({
  selector: 'app-cookies',
  standalone: true,
  templateUrl: './cookies.component.html',
  styleUrl: './cookies.component.scss'
})
export class CookiesComponent implements OnInit {
  page: PageDto | null = null;
  private pagesApi = inject(PagesApiService);

  ngOnInit(): void {
    this.pagesApi.getCookiesPage().subscribe({
      next: (response) => {
        this.page = response.pageData;
      },
      error: (err) => {
        console.error('Failed to load cookies page', err);
      }
    });
  }
}
