import {
  Component,
  ElementRef,
  AfterViewInit,
  HostListener,
  ViewChild,
  inject,
  signal
} from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { AVAILABLE_LANGS, Language } from '../../../i18n/languages.enum';

@Component({
  selector: 'app-language-switcher',
  standalone: true,
  templateUrl: './language-switcher.component.html',
  styleUrls: ['./language-switcher.component.scss']
})
export class LanguageSwitcherComponent implements AfterViewInit {
  private trans = inject(TranslocoService);
  AVAILABLE_LANGS = AVAILABLE_LANGS;

  current = signal<Language>(this.trans.getActiveLang() as Language);
  open = signal(false);
  buttonWidth = signal(0);

  @ViewChild('btn', { read: ElementRef }) btn?: ElementRef<HTMLButtonElement>;

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
