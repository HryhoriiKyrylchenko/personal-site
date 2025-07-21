import {
  ApplicationConfig, inject,
  isDevMode, PLATFORM_ID, provideAppInitializer,
  provideBrowserGlobalErrorListeners,
  provideZoneChangeDetection
} from '@angular/core';
import { provideRouter } from '@angular/router';
import {provideTransloco, translocoConfig, TranslocoService} from '@ngneat/transloco';
import {provideTranslocoPersistLang, cookiesStorage} from '@ngneat/transloco-persist-lang';
import { StaticLoader } from './core/i18n/transloco.loader';
import { routes } from './app.routes';
import { provideClientHydration, withEventReplay } from '@angular/platform-browser';
import {HTTP_INTERCEPTORS, provideHttpClient, withFetch} from '@angular/common/http';
import {AcceptLanguageInterceptor} from './core/interceptors/accept-language.interceptor';
import {firstValueFrom, of} from 'rxjs';
import {catchError} from 'rxjs/operators';
import {LanguageService} from './core/services/language.service';
import {isPlatformBrowser} from '@angular/common';


export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(withFetch()),

    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),

    provideRouter(routes),
    provideClientHydration(withEventReplay()),

    {
      provide: LanguageService,
      useFactory: () => {
        const platformId = inject(PLATFORM_ID);
        const transloco = inject(TranslocoService);

        return {
          getServerLanguage: () => {
            if (isPlatformBrowser(platformId)) {
              return (
                transloco.getActiveLang() ||
                transloco.getDefaultLang() ||
                'en'
              );
            }
            return 'en';
          },
        } as LanguageService;
      },
    },

    provideAppInitializer(() => {
      const langService = inject(LanguageService);
      const transloco = inject(TranslocoService);
      const lang = langService.getServerLanguage();
      transloco.setDefaultLang(lang);
      transloco.setActiveLang(lang);

      return firstValueFrom(
        transloco.load(lang).pipe(
          catchError(err => {
            console.error('Transloco failed to load', err);
            return of(null);
          })
        )
      );
    }),

    provideTransloco({
      loader: StaticLoader,
      config: translocoConfig({
        availableLangs: ['en', 'pl', 'ru', 'uk'],
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
