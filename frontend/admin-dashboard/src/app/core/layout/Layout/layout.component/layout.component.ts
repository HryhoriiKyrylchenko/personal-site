import {Component} from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {CommonModule, } from '@angular/common';

import {
  LoadingSpinnerComponent
} from '../../../../shared/components/loading-spinner/loading-spinner.component/loading-spinner.component';
import {HeaderComponent} from '../../Header/header.component/header.component';
import {FooterComponent} from '../../Footer/footer.component/footer.component';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent, FooterComponent, CommonModule, LoadingSpinnerComponent],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss'
})
export class LayoutComponent {

}
