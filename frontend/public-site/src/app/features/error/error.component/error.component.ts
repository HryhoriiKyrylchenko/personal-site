import {Component} from '@angular/core';
import {TranslocoPipe} from '@ngneat/transloco';

@Component({
  selector: 'app-error',
  standalone: true,
  imports: [
    TranslocoPipe
  ],
  templateUrl: './error.component.html',
  styleUrl: './error.component.scss'
})
export class ErrorComponent {
  reload(): void {
    window.location.reload();
  }
}
