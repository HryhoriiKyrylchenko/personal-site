import {Component, inject, OnInit} from '@angular/core';
import {PageDto} from '../../../shared/models/page-dtos';
import {PagesApiService} from '../../../core/services/pages-api.service';

@Component({
  selector: 'app-privacy',
  standalone: true,
  imports: [],
  templateUrl: './privacy.component.html',
  styleUrl: './privacy.component.scss'
})
export class PrivacyComponent implements OnInit {
  page: PageDto | null = null;
  private pagesApi = inject(PagesApiService);

  ngOnInit(): void {
    this.pagesApi.getPrivacyPage().subscribe({
      next: (response) => {
        this.page = response.pageData;
      },
      error: (err) => {
        console.error('Failed to load privacy page', err);
      }
    });
  }
}
