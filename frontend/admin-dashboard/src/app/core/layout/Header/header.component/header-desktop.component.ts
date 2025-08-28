import {Component} from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterLink} from '@angular/router';


@Component({
  selector: 'app-header-desktop',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './header-desktop.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderDesktopComponent {

}
