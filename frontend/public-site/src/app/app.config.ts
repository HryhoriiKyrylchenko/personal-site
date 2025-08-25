import {
  ApplicationConfig, inject,
  isDevMode, provideAppInitializer,
  provideBrowserGlobalErrorListeners,
  provideZoneChangeDetection
} from '@angular/core';
import { provideRouter } from '@angular/router';
import {provideTransloco, translocoConfig, TranslocoService} from '@ngneat/transloco';
import {provideTranslocoPersistLang} from '@ngneat/transloco-persist-lang';
import { StaticLoader } from './core/i18n/transloco.loader';
import { routes } from './app.routes';
import {
  HTTP_INTERCEPTORS,
  provideHttpClient,
  withFetch, withInterceptors
} from '@angular/common/http';
import {SiteInfoService} from './core/services/site-info.service';
import {localeInterceptor} from './interceptors/locale.interceptor.fn';
import {firstValueFrom} from 'rxjs';
import {catchError, switchMap} from 'rxjs/operators';
import {cookiesStorageRoot} from './shared/utils/cookie-storage.helper';
import {LoadingInterceptor} from './interceptors/loading.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideTranslocoPersistLang({
      storage: { useValue: cookiesStorageRoot() },
      getLangFn: ({ cachedLang, browserLang, cultureLang, defaultLang }) => {
        const mapLang = (lang?: string) => {
          if (!lang) return undefined;
          lang = lang.toLowerCase();
          if (lang.startsWith('en')) return 'en';
          if (lang.startsWith('pl')) return 'pl';
          if (lang.startsWith('ru')) return 'ru';
          if (lang.startsWith('uk')) return 'uk';
          return 'en';
        };

        return cachedLang || mapLang(cultureLang) || mapLang(browserLang) || defaultLang;
      }
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
    {
      provide: HTTP_INTERCEPTORS,
      useClass: LoadingInterceptor,
      multi: true,
    },
  ]
};
