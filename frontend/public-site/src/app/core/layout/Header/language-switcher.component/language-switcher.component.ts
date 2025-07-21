import {
  Component,
  ElementRef,
  AfterViewInit,
  HostListener,
  ViewChild,
  inject,
  signal
} from '@angular/core';
import { TranslocoService} from '@ngneat/transloco';
import { AVAILABLE_LANGS, Language } from '../../../i18n/languages.enum';
import { BreakpointObserver } from '@angular/cdk/layout';
import { toSignal } from '@angular/core/rxjs-interop';
import {map} from 'rxjs';
import {CustomBreakpoints} from '../../../../shared/utils/custom-breakpoints';

@Component({
  selector: 'app-language-switcher',
  standalone: true,
  templateUrl: './language-switcher.component.html',
  styleUrls: ['./language-switcher.component.scss'],
})
export class LanguageSwitcherComponent implements AfterViewInit {
  private trans = inject(TranslocoService);
  private bp = inject(BreakpointObserver);

  AVAILABLE_LANGS = AVAILABLE_LANGS;

  current = signal<Language>(this.trans.getActiveLang() as Language);
  open = signal(false);
  buttonWidth = signal(0);

  @ViewChild('btn', { read: ElementRef }) btn?: ElementRef<HTMLButtonElement>;

  isMobile = toSignal(
    this.bp.observe([CustomBreakpoints.Mobile])
      .pipe(map(res => res.matches)),
    { initialValue: false }
  );

  isMobileOrTablet = toSignal(
    this.bp.observe([CustomBreakpoints.Mobile, CustomBreakpoints.Tablet])
      .pipe(map(res => res.matches)),
  )

  ngAfterViewInit() {
    if (this.btn) {
      this.buttonWidth.set(this.btn.nativeElement.offsetWidth);
    }
  }

  toggle() {
    const nextOpen = !this.open();
    this.open.set(nextOpen);
    if (nextOpen && this.btn) {
      this.buttonWidth.set(this.btn.nativeElement.offsetWidth);
    }
  }

  select(lang: Language) {
    this.trans.setActiveLang(lang);
    this.current.set(lang);
    this.open.set(false);
  }

  currentLangName(): string {
    const map: Record<Language, string> = {
      en: 'English',
      pl: 'Polski',
      ru: 'Русский',
      uk: 'Українська'
    };
    return map[this.current()];
  }

  currentLangNameFor(lang: Language): string {
    const map: Record<Language, string> = {
      en: 'English',
      pl: 'Polski',
      ru: 'Русский',
      uk: 'Українська'
    };
    return map[lang];
  }

  @HostListener('document:click', ['$event'])
  clickOutside(event: MouseEvent) {
    const target = event.target;
    if (this.open() && target instanceof HTMLElement && !this.btn?.nativeElement.contains(target)) {
      this.open.set(false);
    }
  }
}
