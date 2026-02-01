import { Component, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {rxResource} from '@angular/core/rxjs-interop';
import {AnalyticsEventDto, AnalyticsService, PaginatedResult} from '../../../core/services/analytics.service';

@Component({
  selector: 'app-analytics-tab',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './analytics-tab.component.html',
  styleUrl: './analytics-tab.component.scss'
})
export class AnalyticsTabComponent {
  private analyticsService = inject(AnalyticsService);

  page = signal(1);
  pageSize = 20;

  filters = signal({
    eventType: '',
    pageSlug: '',
    from: '',
    to: ''
  });

  selectedIds = signal(new Set<string>());

  dataResource = rxResource({
    params: computed(() => ({ page: this.page(), pageSize: this.pageSize, ...this.filters() })),
    stream: ({ params }) => this.analyticsService.fetchEvents(
      params.page, params.pageSize, params.eventType, params.pageSlug, params.from, params.to
    ),
    defaultValue: {
      items: [], totalCount: 0, pageNumber: 1, pageSize: this.pageSize, totalPages: 0, hasPrevious: false, hasNext: false
    } as PaginatedResult<AnalyticsEventDto>
  });

  data = computed(() => this.dataResource.value());

  totalCount = computed(() => this.data()?.totalCount ?? 0);

  applyFilters() {
    this.page.set(1);
    this.dataResource.reload();
  }

  deleteEvent(id: string) {
    if (!confirm('Delete this event?')) return;
    this.analyticsService.deleteEvent(id).subscribe(() => this.dataResource.reload());
  }

  deleteSelected() {
    const ids = Array.from(this.selectedIds());
    if (ids.length === 0) return;
    if (!confirm(`Delete ${ids.length} selected events?`)) return;
    this.analyticsService.deleteEventsRange(ids).subscribe(() => {
      this.selectedIds.set(new Set());
      this.dataResource.reload();
    });
  }

  toggleSelection(id: string) {
    const set = new Set(this.selectedIds());
    set.has(id) ? set.delete(id) : set.add(id);
    this.selectedIds.set(set);
  }

  prevPage() {
    if (this.data()?.hasPrevious) {
      this.page.update((v) => v - 1);
      this.dataResource.reload();
    }
  }

  nextPage() {
    if (this.data()?.hasNext) {
      this.page.update((v) => v + 1);
      this.dataResource.reload();
    }
  }
}
