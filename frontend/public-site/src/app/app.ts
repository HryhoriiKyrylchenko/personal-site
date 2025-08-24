import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { inject, Injectable } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
import {
  CookiesBannerComponent
} from './shared/components/cookies-banner/cookies-banner.component/cookies-banner.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CookiesBannerComponent,
    RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
@Injectable({ providedIn: 'root' })
export class App {
  private titleService = inject(Title);
  private router = inject(Router);
  private activatedRoute = inject(ActivatedRoute);
  private title = 'Hryhorii Kyrylchenko';

  constructor() {
    this.router.events
      .pipe(
        filter(event => event instanceof NavigationEnd)
      )
      .subscribe(() => {
        const route = this.getChild(this.activatedRoute);
        route.data.subscribe(data => {
          this.titleService.setTitle(data['title'] ? data['title'] + " | " + this.title : this.title);
        });
      });
  }

  private getChild(route: ActivatedRoute): ActivatedRoute {
    return route.firstChild ? this.getChild(route.firstChild) : route;
  }
}
