import { Component } from '@angular/core';
import {LogsTabComponent} from '../logs-tab.component/logs-tab.component';
import {AnalyticsTabComponent} from '../analytics-tab.component/analytics-tab.component';

@Component({
  selector: 'app-analytics',
  standalone: true,
  imports: [
    LogsTabComponent,
    AnalyticsTabComponent
  ],
  templateUrl: './analytics.component.html',
  styleUrl: './analytics.component.scss'
})
export class AnalyticsComponent {
  selectedTab = 0;

  tabs = [
    { label: 'Analytics' },
    { label: 'Logs' }
  ];
}
