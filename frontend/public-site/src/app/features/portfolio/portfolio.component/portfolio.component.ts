import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PagesApiService } from '../../../core/services/pages-api.service';
import { PortfolioPageDto } from '../../../shared/models/page-dtos';

@Component({
  selector: 'app-portfolio',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './portfolio.component.html',
  styleUrls: ['./portfolio.component.scss']
})
export class PortfolioComponent {
  private pagesApi = inject(PagesApiService);
  data?: PortfolioPageDto;

  constructor() {
    //this.pagesApi.getPortfolioPage().subscribe(dto => { this.data = dto; });
  }
}
