import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TranslocoLoader } from '@ngneat/transloco';
import { of, take} from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class StaticLoader implements TranslocoLoader {
  private http = inject(HttpClient);

  getTranslation(lang: string) {
    return this.http.get<Record<string, unknown>>(`/assets/i18n/${lang}.json`)
      .pipe(take(1), catchError(() => of({})));
  }
}
