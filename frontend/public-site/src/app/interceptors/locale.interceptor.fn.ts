import { HttpInterceptorFn, HttpRequest, HttpHandlerFn } from '@angular/common/http';
import {inject} from '@angular/core';
import {TranslocoService} from '@ngneat/transloco';

export const localeInterceptor: HttpInterceptorFn = (req: HttpRequest<unknown>, next: HttpHandlerFn) => {
  const transloco = inject(TranslocoService);

  const lang = transloco.getActiveLang() ?? transloco.getDefaultLang() ?? 'en';

  const cloned = req.clone({
    headers: req.headers.set('X-Locale', lang),
    withCredentials: true,
  });

  return next(cloned);
};
