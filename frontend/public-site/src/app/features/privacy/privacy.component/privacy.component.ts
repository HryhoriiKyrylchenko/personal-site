import {Component, inject, OnInit} from '@angular/core';
import {PageDto} from '../../../shared/models/page-dtos';
import {PagesApiService} from '../../../core/services/pages-api.service';
import {TranslocoService} from '@ngneat/transloco';
import {Subscription} from 'rxjs';

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
