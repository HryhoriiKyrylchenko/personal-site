import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import {
  PageService,
  PageAdminDto,
  PageTranslationDto
} from '../../../core/services/page.service';

@Component({
  selector: 'app-pages-tab',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './pages-tab.component.html',
  styleUrl: './pages-tab.component.scss'
})
export class PagesTabComponent implements OnInit {
  private pageService = inject(PageService);

  pages = signal<PageAdminDto[]>([]);
  selectedPageId = signal<string | null>(null);
  editingPage = signal<PageAdminDto | null>(null);

  isEditing = signal(false);
  private originalPage: PageAdminDto | null = null;

  activeLang = 'en';
  readonly languages = ['en', 'pl', 'ru', 'uk'];

  ngOnInit(): void {
    this.loadPages();
  }

  /* ================= LOAD ================= */

  loadPages() {
    this.pageService.getAll().subscribe(res => {
      this.pages.set(res);
      if (!this.selectedPageId() && res.length > 0 && res[0].id) {
        this.selectPage(res[0].id);
      }
    });
  }

  selectPage(pageId: string) {
    this.selectedPageId.set(pageId);

    const page = this.pages().find(p => p.id === pageId);
    if (!page) return;

    this.originalPage = structuredClone(page);
    this.editingPage.set(structuredClone(page));
    this.isEditing.set(false);
  }


  /* ================= CREATE ================= */

  createPage() {
    this.selectedPageId.set(null);
    this.originalPage = null;

    this.editingPage.set({
      key: '',
      translations: this.emptyTranslations()
    });

    this.isEditing.set(true);
  }

  editPage() {
    if (!this.editingPage()) return;
    this.isEditing.set(true);
  }

  /* ================= SAVE / CANCEL ================= */

  savePage() {
    const page = this.editingPage();
    if (!page) return;

    const action = page.id
      ? this.pageService.update(page)
      : this.pageService.create(page);

    action.subscribe(() => {
      this.isEditing.set(false);
      this.loadPages();
    });
  }

  cancelEdit() {
    this.isEditing.set(false);

    if (this.originalPage) {
      this.editingPage.set(structuredClone(this.originalPage));
    }
    const firstPage = this.pages()[0];

    if (!this.selectedPageId() && firstPage && firstPage.id) {
      this.selectPage(firstPage.id);
    }
  }

  deletePage(id: string) {
    if (!confirm('Delete page?')) return;
    this.pageService.delete(id).subscribe(() => {
      this.editingPage.set(null);
      this.loadPages();
    });
  }

  /* ================= TRANSLATIONS ================= */

  currentTranslation(): PageTranslationDto | undefined {
    return this.editingPage()?.translations
      .find(t => t.languageCode === this.activeLang);
  }

  private emptyTranslations(): PageTranslationDto[] {
    return this.languages.map(l => ({
      id: '',
      pageId: '',
      languageCode: l,
      data: '',
      title: '',
      description: '',
      metaTitle: '',
      metaDescription: '',
      ogImage: ''
    }));
  }
}
