import {Component} from '@angular/core';

@Component({
  selector: 'app-error',
  standalone: true,
  templateUrl: './error.component.html',
  styleUrl: './error.component.scss'
})
export class ErrorComponent {
  reload(): void {
    window.location.reload();
  }
}
