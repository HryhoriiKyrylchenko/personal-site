import { Injectable } from "@angular/core";
import {CanActivate, Router} from '@angular/router';
import { AuthService } from "./auth.service";
import {map, Observable} from "rxjs";

@Injectable({ providedIn: 'root' })
export class PasswordChangedGuard implements CanActivate {
  constructor(
    private auth: AuthService,
    private router: Router
  ) {}

  canActivate(): Observable<boolean> {
    return this.auth.me().pipe(
      map(user => {
        if (user.mustChangePassword) {
          void this.router.navigate(['change-password']);
          return false;
        }
        return true;
      })
    );
  }
}
