import {Component, inject} from '@angular/core';
import {FormBuilder, ReactiveFormsModule, Validators} from '@angular/forms';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import {AuthService} from '../../../core/auth/auth.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  imports: [
    ReactiveFormsModule
  ],
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent {
  isSubmitting = false;
  error: string | null = null;

  private fb = inject(FormBuilder);
  private auth = inject(AuthService);
  private router = inject(Router);

  form = this.fb.group({
    currentPassword: ['', Validators.required],
    newPassword: ['', [Validators.required, Validators.minLength(8)]],
    confirmPassword: ['', Validators.required]
  });

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const { currentPassword, newPassword, confirmPassword } =
      this.form.getRawValue();

    if (!currentPassword || !newPassword || !confirmPassword) {
      this.error = 'Passwords do not match';
      return;
    }

    if (newPassword !== confirmPassword) {
      this.error = 'Passwords do not match';
      return;
    }

    this.error = null;
    this.isSubmitting = true;

    this.auth
      .changePassword(currentPassword!, newPassword!)
      .subscribe({
        next: () => {
          void this.router.navigate(['']);
        },
        error: (err) => {
          this.error =
            err?.error?.message ?? 'Failed to change password';
          this.isSubmitting = false;
        }
      });
  }
}
