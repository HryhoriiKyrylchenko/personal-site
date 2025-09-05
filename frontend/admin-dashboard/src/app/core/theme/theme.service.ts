import { Injectable, inject } from '@angular/core';
import { PLATFORM_ID, DOCUMENT } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

@Injectable({ providedIn: 'root' })
export class ThemeService {
  private platformId = inject(PLATFORM_ID);
  private document = inject(DOCUMENT);
  private isBrowser = isPlatformBrowser(this.platformId);
  private readonly storageKey = 'theme';

  constructor() {
    const saved = this.isBrowser
      ? localStorage.getItem(this.storageKey)
      : null;
    this.applyTheme(saved === 'dark' ? 'dark' : 'light');
  }

  private applyTheme(theme: 'light' | 'dark') {
    if (this.isBrowser) {
      this.document.body.setAttribute('data-theme', theme);
      localStorage.setItem(this.storageKey, theme);
    }
  }

  toggleTheme() {
    const current = this.isBrowser
      ? this.document.body.getAttribute('data-theme') as 'light' | 'dark'
      : 'light';
    this.applyTheme(current === 'light' ? 'dark' : 'light');
  }

  getTheme() {
    return this.isBrowser
      ? this.document.body.getAttribute('data-theme') as 'light' | 'dark'
      : 'light';
  }
}
