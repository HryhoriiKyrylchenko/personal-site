import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {
  ProjectService,
  ProjectAdminDto,
  ProjectTranslationDto, ProjectSkillAdminDto
} from '../../../core/services/project.service';
import {
  SkillService,
  SkillAdminDto,
} from '../../../core/services/skills.service';
import {FileUploadService, UploadFolder} from '../../../core/services/file-upload.service';

@Component({
  selector: 'app-project-tab',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './projects-tab.component.html',
  styleUrl: './projects-tab.component.scss'
})
export class ProjectsTabComponent implements OnInit {
  private projectService = inject(ProjectService);
  private skillService = inject(SkillService);
  private readonly fileUploadService = inject(FileUploadService);

  projects = signal<ProjectAdminDto[]>([]);
  skills = signal<SkillAdminDto[]>([]);
  page = signal(1);
  pageSize = 10;
  totalPages = signal(1);
  slugFilter = signal('');
  editingProject = signal<ProjectAdminDto | null>(null);

  activeLang = 'en';

  ngOnInit() {
    this.loadProjects();
    this.loadSkills();
  }

  loadSkills() {
    this.skillService
      .getAll()
      .subscribe(res => {
        if (res != null)
        {
          this.skills.set(res);
        }
      });
  }

  loadProjects() {
    this.projectService
      .getAll(this.page(), this.pageSize, this.slugFilter())
      .subscribe(res => {
        if (res.isSuccess) {
          this.projects.set(res.value);
          this.totalPages.set(res.totalPages);
        }
      });
  }

  prevPage() {
    if (this.page() > 1) {
      this.page.update(p => p - 1);
      this.loadProjects();
    }
  }

  nextPage() {
    if (this.page() < this.totalPages()) {
      this.page.update(p => p + 1);
      this.loadProjects();
    }
  }

  openCreate() {
    this.editingProject.set({
      id: '',
      slug: '',
      coverImage: '',
      demoUrl: '',
      repoUrl: '',
      createdAt: new Date().toISOString(),
      updatedAt: undefined,
      translations: this.createEmptyTranslations(),
      skills: []
    });
  }

  editProject(project: ProjectAdminDto) {
    this.editingProject.set(JSON.parse(JSON.stringify(project)));
  }

  cancelEdit() {
    this.editingProject.set(null);
  }

  saveProject() {
    const project = this.editingProject();
    if (!project) return;

    const request = project.id
      ? this.projectService.update(project.id, project)
      : this.projectService.create(project);

    request.subscribe(() => {
      this.loadProjects();
      this.cancelEdit();
    });
  }

  deleteProject(id: string) {
    if (!confirm('Delete this project?')) return;
    this.projectService.delete(id).subscribe(() => this.loadProjects());
  }

  private createEmptyTranslations(): ProjectTranslationDto[] {
    return ['en', 'pl', 'ru', 'uk'].map(lang => ({
      languageCode: lang,
      title: '',
      shortDescription: '',
      descriptionSections: '{"Overview":"","Technologies":"","Features":"","Architecture":"","Deployment":"","Results":""}',
      metaTitle: '',
      metaDescription: '',
      ogImage: ''
    } as any));
  }

  get slugFilterValue() {
    return this.slugFilter();
  }
  set slugFilterValue(val: string) {
    this.slugFilter.set(val);
  }

  addSkill(skillId: string) {
    if (!skillId) return;

    const skill = this.skills().find(s => s.id === skillId);
    if (!skill) return;

    const project = this.editingProject();
    if (!project) return;

    const alreadyAdded = project.skills.some(
      s => s.skill.id === skill.id
    );

    if (alreadyAdded) return;

    const projectSkill: ProjectSkillAdminDto = {
      id: '00000000-0000-0000-0000-000000000000',
      projectId: project.id ?? '00000000-0000-0000-0000-000000000000',
      skill: skill
    };

    project.skills.push(projectSkill);
  }

  removeSkill(skillId: string) {
    const project = this.editingProject();
    if (!project) return;

    project.skills = project.skills.filter(
      s => s.skill.id !== skillId
    );
  }

  uploadCoverImage(event: Event) {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (!file) return;

    this.fileUploadService
      .upload(file, UploadFolder.Projects)
      .subscribe(url => {
        this.editingProject.update(p => {
          if (!p) return p;
          p.coverImage = url;
          return p;
        });
      });
  }

  deleteCoverImage() {
    const url = this.editingProject()?.coverImage;
    if (!url) return;

    this.fileUploadService.delete(url).subscribe(() => {
      this.editingProject.update(p => {
        if (!p) return p;
        p.coverImage = '';
        return p;
      });
    });
  }

  uploadOgImage(event: Event, languageCode: string) {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (!file) return;

    this.fileUploadService
      .upload(file, UploadFolder.Seo)
      .subscribe(url => {
        this.editingProject.update(p => {
          if (!p) return p;
          const tr = p.translations.find(t => t.languageCode === languageCode);
          if (tr) tr.ogImage = url;
          return p;
        });
      });
  }

  deleteOgImage(languageCode: string) {
    const tr = this.editingProject()?.translations.find(t => t.languageCode === languageCode);
    if (!tr?.ogImage) return;

    this.fileUploadService.delete(tr.ogImage).subscribe(() => {
      this.editingProject.update(p => {
        if (!p) return p;
        const t = p.translations.find(x => x.languageCode === languageCode);
        if (t) t.ogImage = '';
        return p;
      });
    });
  }
}
