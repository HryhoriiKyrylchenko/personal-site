import {Component, computed, inject, signal} from '@angular/core';
import {LogEntryDto, LogsService} from "../../../core/services/logs.service";
import {FormsModule} from '@angular/forms';
import {DatePipe, JsonPipe} from '@angular/common';
import {rxResource} from '@angular/core/rxjs-interop';
import {PaginatedResult} from '../../../core/services/analytics.service';

@Component({
  selector: 'app-logs-tab',
  standalone: true,
  imports: [
    FormsModule,
    JsonPipe,
    DatePipe
  ],
  templateUrl: './logs-tab.component.html',
  styleUrl: './logs-tab.component.scss'
})
export class LogsTabComponent {
  private logsService = inject(LogsService);

  page = signal(1);
  pageSize = 20;

  fromDate = signal('');
  toDate = signal('');
  level = signal(0);

  private _cutoffDate = signal('');

  get cutoffDate() {
    return this._cutoffDate();
  }
  set cutoffDate(value: string) {
    this._cutoffDate.set(value);
  }

  dataResource = rxResource({
    params: computed(() => ({
      page: this.page(),
      pageSize: this.pageSize,
      from: this.fromDate(),
      to: this.toDate(),
      level: this.level()
    })),
    stream: ({ params }) =>
      this.logsService.getLogsPaginated(
        params.page,
        params.pageSize,
        params.from || undefined,
        params.to || undefined,
        params.level || undefined
      ),
    defaultValue: {
      items: [],
      totalCount: 0,
      pageNumber: 1,
      pageSize: this.pageSize,
      totalPages: 0,
      hasPrevious: false,
      hasNext: false
    } as PaginatedResult<LogEntryDto>
  });

  data = computed(() => this.dataResource.value());
  totalCount = computed(() => this.data()?.totalCount ?? 0);

  reload() {
    this.dataResource.reload();
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

  applyFilters() {
    this.page.set(1);
    this.dataResource.reload();
  }

  resetFilters() {
    this.fromDate.set('');
    this.toDate.set('');
    this.level.set(0);
    this.page.set(1);
    this.dataResource.reload();
  }

  deleteOlderThan(): void {
    if (!this.cutoffDate) return;
    const isoDate = new Date(this.cutoffDate).toISOString();
    if (!confirm(`Delete logs older than ${this.cutoffDate}?`)) return;
    this.logsService.deleteOlderThan(isoDate).subscribe({
      next: () => this.dataResource.reload(),
      error: err => console.error(err)
    });
  }
}
