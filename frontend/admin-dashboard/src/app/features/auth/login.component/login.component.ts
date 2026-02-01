import {Component} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormsModule} from '@angular/forms';
import {AuthService} from '../../../core/auth/auth.service';
import {Router} from '@angular/router';


@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  username = '';
  password = '';
  error?: string;

  constructor(
    private auth: AuthService,
    private router: Router
  ) {}

  submit(): void {
    this.auth.login(this.username, this.password).subscribe({
      next: res => {
        if (res.mustChangePassword) {
          void this.router.navigate(['/change-password']);
        } else {
          void this.router.navigate(['/']);
        }
      },
      error: () => {
        this.error = 'Invalid credentials';
      }
    });
  }
}
