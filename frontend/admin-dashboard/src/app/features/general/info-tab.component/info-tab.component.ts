import {Component, inject, OnInit, signal} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { LanguageService, LanguageDto } from '../../../core/services/language.service';
import { ResumeService, ResumeDto } from '../../../core/services/resume.service';
import { SocialMediaLinkService, SocialMediaLinkDto } from '../../../core/services/social-media-link.service';
import {FileUploadService, UploadFolder} from '../../../core/services/file-upload.service';

@Component({
  selector: 'app-info-tab',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './info-tab.component.html',
  styleUrls: ['./info-tab.component.scss']
})
export class InfoTabComponent implements OnInit {
  private languageService = inject(LanguageService);
  private resumeService = inject(ResumeService);
  private socialService = inject(SocialMediaLinkService);
  private readonly fileUploadService = inject(FileUploadService);

  // ================= SIGNALS =================
  languages = signal<LanguageDto[]>([]);
  editingLanguage = signal<LanguageDto | null>(null);

  activeResume = signal<ResumeDto | null>(null);
  editingResume = signal<ResumeDto | null>(null);
  newResumeFileUrl = signal('');
  newResumeFileName = signal('');

  socialLinks = signal<SocialMediaLinkDto[]>([]);
  editingSocialLink = signal<SocialMediaLinkDto | null>(null);

  ngOnInit() {
    this.loadAll();
  }

  loadAll() {
    this.loadLanguages();
    this.loadResume();
    this.loadSocialLinks();
  }

  // ================= LANGUAGES =================
  loadLanguages() {
    this.languageService.getAll().subscribe(res => {
      this.languages.set(Array.isArray(res) ? res : [])
    });
  }

  editLanguage(lang: LanguageDto) {
    this.editingLanguage.set({ ...lang });
  }

  createLanguage() {
    this.editingLanguage.set({ id: '', code: '', name: '' });
  }

  saveLanguage() {
    const lang = this.editingLanguage();
    if (!lang) return;

    const action = lang.id
      ? this.languageService.update(lang)
      : this.languageService.create(lang.code, lang.name);

    action.subscribe(() => {
      this.loadLanguages();
      this.editingLanguage.set(null);
    });
  }

  cancelLanguageEdit() {
    this.editingLanguage.set(null);
  }

  deleteLanguage(id: string) {
    if (!confirm('Delete language?')) return;
    this.languageService.delete(id).subscribe(() => this.loadLanguages());
  }

  // ================= RESUME =================
  loadResume() {
    this.resumeService.getAll().subscribe(res => {
      const active = res.value?.find(r => r.isActive) ?? null;
      this.activeResume.set(active);
    });
  }

  startResumeEdit() {
    const r = this.activeResume();
    this.newResumeFileUrl.set(r?.fileUrl ?? '');
    this.newResumeFileName.set(r?.fileName ?? '');
    this.editingResume.set(r);
  }

  uploadResume(event: Event) {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (!file) return;

    this.fileUploadService
      .upload(file, UploadFolder.Resumes)
      .subscribe(url => {
        this.editingResume.update(r => {
          if (!r) return r;
          r.fileUrl = url;
          r.fileName = url.split('/').pop() ?? '';
          r.uploadedAt = new Date().toISOString();
          return r;
        });
      });
  }

  saveResume() {
    const resume = this.activeResume();
    if (!resume) return;

    const action = resume.id
      ? this.resumeService.update(resume)
      : this.resumeService.create(resume.fileUrl, resume.fileName, resume.isActive);

    action.subscribe(() => {
      this.loadResume();
      this.editingResume.set(null);
    });
  }

  cancelResumeEdit() {
    this.editingResume.set(null);
    this.loadResume();
  }

  // ================= SOCIAL LINKS =================
  loadSocialLinks() {
    this.socialService.getAll().subscribe(res => {
      this.socialLinks.set(res.value ?? [])
    });
  }

  editSocialLink(link: SocialMediaLinkDto) {
    this.editingSocialLink.set({ ...link });
  }

  createSocialLink() {
    this.editingSocialLink.set({ id: '', platform: '', url: '', displayOrder: 0, isActive: false });
  }

  saveSocialLink() {
    const s = this.editingSocialLink();
    if (!s) return;

    const action = s.id
      ? this.socialService.update(s)
      : this.socialService.create(s.platform, s.url, s.displayOrder, s.isActive);

    action.subscribe(() => {
      this.loadSocialLinks();
      this.editingSocialLink.set(null);
    });
  }

  cancelSocialEdit() {
    this.editingSocialLink.set(null);
  }

  deleteSocialLink(id: string) {
    if (!confirm('Delete social link?')) return;
    this.socialService.delete(id).subscribe(() => this.loadSocialLinks());
  }

  toggleSocialActive(link: SocialMediaLinkDto) {
    this.socialService.update({ ...link, isActive: !link.isActive })
      .subscribe(() => this.loadSocialLinks());
  }
}
