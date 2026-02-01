import {Component, inject} from '@angular/core';
import {AuthService} from '../../../auth/auth.service';
import {Router} from '@angular/router';


@Component({
  selector: 'app-account-buttons',
  standalone: true,
  templateUrl: './account-buttons.component.html',
  styleUrls: ['./account-buttons.component.scss']
})
export class AccountButtonsComponent {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  logout(): void {
    this.authService.logout().subscribe({
      next: () => {
        this.router.navigate(['/login']).then();
      }
    });
  }

  openAccount(): void {
    this.router.navigate(['/account']).then();
  }
}
