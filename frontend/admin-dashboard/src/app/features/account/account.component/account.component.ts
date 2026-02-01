import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {AuthService} from '../../../core/auth/auth.service';

@Component({
  standalone: true,
  selector: 'app-account',
  imports: [FormsModule],
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss']
})
export class AccountComponent {
  private readonly authService = inject(AuthService);

  currentPassword = '';
  newPassword = '';
  confirmPassword = '';

  passwordMismatch(): boolean {
    return (
      this.newPassword.length > 0 &&
      this.confirmPassword.length > 0 &&
      this.newPassword !== this.confirmPassword
    );
  }

  changePassword(): void {
    if (this.passwordMismatch()) return;

    this.authService
      .changePassword(this.currentPassword, this.newPassword)
      .subscribe(() => {
        this.currentPassword = '';
        this.newPassword = '';
        this.confirmPassword = '';
        alert('Password changed');
      });
  }
}
