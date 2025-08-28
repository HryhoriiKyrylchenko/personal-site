import {Component, inject, OnDestroy, OnInit} from '@angular/core';
import {MatProgressSpinner} from '@angular/material/progress-spinner';
import {LoadingService} from '../../../../core/services/loading.service';
import {Subscription} from 'rxjs';

@Component({
  selector: 'app-loading-spinner',
  standalone: true,
  imports: [
    MatProgressSpinner
  ],
  templateUrl: './loading-spinner.component.html',
  styleUrl: './loading-spinner.component.scss'
})
export class LoadingSpinnerComponent implements OnInit, OnDestroy {
  isLoading = false;
  private loadingSubscription: Subscription | null = null;
  private loadingService = inject(LoadingService);

  ngOnInit() {
    this.loadingSubscription = this.loadingService.loading$.subscribe(
      (loading) => {
        this.isLoading = loading;
      }
    );
  }

  ngOnDestroy() {
    if (this.loadingSubscription) {
      this.loadingSubscription.unsubscribe();
    }
  }
}
