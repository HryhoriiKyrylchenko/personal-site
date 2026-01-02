import { Component } from '@angular/core';
import {ProjectsTabComponent} from '../projects-tab.component/projects-tab.component';

@Component({
  selector: 'app-portfolio',
  standalone: true,
  imports: [
    ProjectsTabComponent
  ],
  templateUrl: './portfolio.component.html',
  styleUrl: './portfolio.component.scss'
})
export class PortfolioComponent {
  selectedTab = 0;

  tabs = [
    { label: 'Projects' }
  ];
}
