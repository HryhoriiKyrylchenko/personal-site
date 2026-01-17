import { Injectable } from "@angular/core";
import { CanActivate, Router } from "@angular/router";
import {AuthService} from './auth.service';
import {catchError, map, Observable, of} from "rxjs";

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(
    private auth: AuthService,
    private router: Router
  ) {}

  canActivate(): Observable<boolean> {
    return this.auth.me().pipe(
      map(() => true),
      catchError(() => {
        void this.router.navigate(['login']);
        return of(false);
      })
    );
  }
}
