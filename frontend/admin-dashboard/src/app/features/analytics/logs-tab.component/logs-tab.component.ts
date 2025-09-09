import {Component, OnInit} from '@angular/core';
import {LogEntryDto, LogsService} from "../../../core/services/logs.service";
import {FormsModule} from '@angular/forms';
import {DatePipe, JsonPipe} from '@angular/common';

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
export class LogsTabComponent implements OnInit {
  logs: LogEntryDto[] = [];
  cutoffDate = '';

  constructor(private logsService: LogsService) {}

  ngOnInit(): void {
    this.loadLogs();
  }

  loadLogs(count = 100): void {
    this.logsService.getRecentLogs(count).subscribe({
      next: data => {
        this.logs = data;
      },
      error: err => {
        console.error(err);
      }
    });
  }

  deleteOlderThan(): void {
    if (!this.cutoffDate) return;
    const isoDate = new Date(this.cutoffDate).toISOString();
    this.logsService.deleteOlderThan(isoDate).subscribe({
      next: _ => {
        this.loadLogs();
      },
      error: err => {
        console.error(err);
      }
    });
  }
}
