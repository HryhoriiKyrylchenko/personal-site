import {
  ApplicationConfig,
  isDevMode,
  provideBrowserGlobalErrorListeners,
  provideZoneChangeDetection
} from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideTransloco, translocoConfig } from '@ngneat/transloco';
import { provideTranslocoPersistLang, cookiesStorage } from '@ngneat/transloco-persist-lang';
import { StaticLoader } from './core/i18n/transloco.loader';
import { routes } from './app.routes';
import { provideClientHydration, withEventReplay } from '@angular/platform-browser';
import {HTTP_INTERCEPTORS, provideHttpClient, withFetch} from '@angular/common/http';
import {AcceptLanguageInterceptor} from './core/interceptors/accept-language.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(withFetch()),

    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),

    provideRouter(routes),
    provideClientHydration(withEventReplay()),

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
    provideTranslocoPersistLang({
      storage: {
        useValue: cookiesStorage()
      }
    }),
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AcceptLanguageInterceptor,
      multi: true
    }
  ]
};
