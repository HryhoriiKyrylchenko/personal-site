import { Injectable, inject } from '@angular/core';
import {
  HttpInterceptor, HttpRequest,
  HttpHandler, HttpEvent
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { TranslocoService } from '@ngneat/transloco';

@Injectable()
export class LocaleInterceptor implements HttpInterceptor {
  private transloco = inject(TranslocoService);

  intercept(req: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    console.log('🏷️ LocaleInterceptor:', this.transloco.getActiveLang(), '->', req.url);
    const lang = this.transloco.getActiveLang();
    const cloned = req.clone({
      headers: req.headers.set('X-Locale', lang),
      withCredentials: true
    });

    return next.handle(cloned);
  }
}
