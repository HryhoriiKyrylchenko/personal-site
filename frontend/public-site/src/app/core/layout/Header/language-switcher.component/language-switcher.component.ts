import {
  Component,
  ElementRef,
  AfterViewInit,
  HostListener,
  ViewChild,
  inject,
  signal, computed
} from '@angular/core';
import { TranslocoService} from '@ngneat/transloco';
import { BreakpointObserver } from '@angular/cdk/layout';
import { toSignal } from '@angular/core/rxjs-interop';
import {map} from 'rxjs';
import {CustomBreakpoints} from '../../../../shared/utils/custom-breakpoints';
import {SiteInfoService} from '../../../services/site-info.service';
import {SUPPORTED_LANG_CODES} from '../../../i18n/transloco.config';

@Component({
  selector: 'app-language-switcher',
  standalone: true,
  templateUrl: './language-switcher.component.html',
  styleUrls: ['./language-switcher.component.scss'],
})
export class LanguageSwitcherComponent implements AfterViewInit {
  private trans = inject(TranslocoService);
  private bp = inject(BreakpointObserver);
  private siteInfoSvc = inject(SiteInfoService);
  private supportedCodes = [...SUPPORTED_LANG_CODES] as string[];

  languages = computed(() => {
    const info = this.siteInfoSvc.siteInfo();
    if (!info?.languages) return [];
    return info.languages.filter(lang => this.supportedCodes.includes(lang.code));
  });

  current = signal(this.trans.getActiveLang() as string);
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

  select(langCode: string) {
    this.trans.setActiveLang(langCode);
    this.current.set(langCode);
    this.open.set(false);
  }

  currentLangName(): string {
    const lang = this.languages().find(l => l.code === this.current());
    return lang ? lang.name : this.current();
  }

  currentLangNameFor(code: string): string {
    const lang = this.languages().find(l => l.code === code);
    return lang ? lang.name : code;
  }

  @HostListener('document:click', ['$event'])
  clickOutside(event: MouseEvent) {
    const target = event.target;
    if (this.open() && target instanceof HTMLElement && !this.btn?.nativeElement.contains(target)) {
      this.open.set(false);
    }
  }
}
