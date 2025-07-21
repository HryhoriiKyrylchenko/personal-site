import {Component, inject, OnInit} from '@angular/core';
import {PageDto} from '../../../shared/models/page-dtos';
import {PagesApiService} from '../../../core/services/pages-api.service';
import {TranslocoService} from '@ngneat/transloco';
import {Subscription} from 'rxjs';

@Component({
  selector: 'app-cookies',
  standalone: true,
  templateUrl: './cookies.component.html',
  styleUrl: './cookies.component.scss'
})
export class CookiesComponent implements OnInit {
  page: PageDto | null = null;
  private pagesApi = inject(PagesApiService);
  private translocoService = inject(TranslocoService);
  private langChangedSubscription!: Subscription;

  ngOnInit(): void {
    this.loadData();

    this.langChangedSubscription = this.translocoService.events$.subscribe(event => {
      if (event.type === 'langChanged') {
        this.loadData();
      }
    });
  }

  ngOnDestroy(): void {
    if (this.langChangedSubscription) {
      this.langChangedSubscription.unsubscribe();
    }
  }

  private loadData(): void {
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
