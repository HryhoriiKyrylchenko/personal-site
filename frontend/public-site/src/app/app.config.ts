import {
  ApplicationConfig, inject,
  isDevMode, provideAppInitializer,
  provideBrowserGlobalErrorListeners,
  provideZoneChangeDetection
} from '@angular/core';
import { provideRouter } from '@angular/router';
import {provideTransloco, translocoConfig, TranslocoService} from '@ngneat/transloco';
import {cookiesStorage, provideTranslocoPersistLang} from '@ngneat/transloco-persist-lang';
import { StaticLoader } from './core/i18n/transloco.loader';
import { routes } from './app.routes';
import { provideClientHydration, withEventReplay } from '@angular/platform-browser';
import {
  provideHttpClient,
  withFetch, withInterceptors
} from '@angular/common/http';
import {SiteInfoService} from './core/services/site-info.service';
import {localeInterceptor} from './interceptors/locale.interceptor.fn';
import {firstValueFrom} from 'rxjs';
import {catchError, switchMap} from 'rxjs/operators';

export const appConfig: ApplicationConfig = {
  providers: [
    provideTranslocoPersistLang({
      storage: { useValue: cookiesStorage() },
      getLangFn: ({ cachedLang, browserLang, cultureLang, defaultLang }) =>
        cachedLang || cultureLang || browserLang?.substring(0, 2) || defaultLang
    }),
    provideTransloco({
      loader: StaticLoader,
      config: translocoConfig({
        availableLangs: ['en', 'pl', 'ru', 'uk'],
        defaultLang: 'en',
        fallbackLang: 'en',
        reRenderOnLangChange: true,
        prodMode: !isDevMode(),
      })
    }),
    provideAppInitializer(() => {
      const transloco = inject(TranslocoService);
      const active = transloco.getActiveLang() ?? transloco.getDefaultLang() ?? 'en';
      return firstValueFrom(
        transloco.load(active).pipe(
          switchMap(() => transloco.selectTranslation(active)),
          catchError(() => transloco.load('en'))
        )
      );
    }),
    provideHttpClient(
      withFetch(),
      withInterceptors([localeInterceptor])
    ),
    provideAppInitializer(() => inject(SiteInfoService).loadSiteInfo()),
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),

    provideRouter(routes),
    provideClientHydration(withEventReplay())
  ]
};
