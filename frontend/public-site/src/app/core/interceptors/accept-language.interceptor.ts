import { Injectable, inject } from '@angular/core';
import {
  HttpInterceptor, HttpRequest,
  HttpHandler, HttpEvent
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { TranslocoService } from '@ngneat/transloco';

@Injectable()
export class AcceptLanguageInterceptor implements HttpInterceptor {
  private transloco = inject(TranslocoService);

  intercept(req: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const lang = this.transloco.getActiveLang();
    const cloned = req.clone({ headers: req.headers.set('Accept-Language', lang) });
    return next.handle(cloned);
  }
}
