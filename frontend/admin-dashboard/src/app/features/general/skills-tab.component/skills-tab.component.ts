import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { SkillService, SkillAdminDto } from '../../../core/services/skills.service';
import { SkillCategoryService, SkillCategoryAdminDto } from '../../../core/services/skill-category.service';
import { UserSkillAdminDto, UserSkillService } from '../../../core/services/user-skills.service';
import {
  LearningSkillAdminDto,
  LearningSkillService,
  LearningStatus
} from '../../../core/services/learning-skills.service';

@Component({
  selector: 'app-skills-tab',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './skills-tab.component.html',
  styleUrl: './skills-tab.component.scss'
})
export class SkillsTabComponent implements OnInit {
  private skillService = inject(SkillService);
  private categoryService = inject(SkillCategoryService);
  private userSkillService = inject(UserSkillService);
  private learningSkillService = inject(LearningSkillService);

  categories = signal<SkillCategoryAdminDto[]>([]);
  skills = signal<SkillAdminDto[]>([]);
  userSkills = signal<UserSkillAdminDto[]>([]);
  learningSkills = signal<LearningSkillAdminDto[]>([]);

  selectedCategoryId = signal<string | null>(null);

  editingCategory = signal<SkillCategoryAdminDto | null>(null);
  editingSkill = signal<SkillAdminDto | null>(null);

  activeLang = 'en';

  learningStatusOptions = [
    { value: LearningStatus.Planned, label: 'Planned' },
    { value: LearningStatus.InProgress, label: 'In Progress' },
    { value: LearningStatus.Practicing, label: 'Practicing' }
  ];

  ngOnInit() {
    this.loadAll();
  }

  loadAll() {
    this.loadCategories();
    this.loadSkills();
    this.loadUserSkills();
    this.loadLearningSkills();
  }

  loadCategories() {
    this.categoryService.getAll().subscribe(res => this.categories.set(res));
  }

  loadSkills() {
    this.skillService.getAll(
      this.selectedCategoryId() ?? undefined
    ).subscribe(res => this.skills.set(res));
  }

  loadUserSkills() {
    this.userSkillService.getAll().subscribe(res => this.userSkills.set(res));
  }

  loadLearningSkills() {
    this.learningSkillService.getAll().subscribe(res => this.learningSkills.set(res));
  }

  createCategory() {
    this.editingCategory.set({
      id: '',
      key: '',
      displayOrder: 0,
      translations: this.emptyCategoryTranslations()
    });
  }

  saveCategory() {
    const category = this.editingCategory();
    if (!category) return;

    const action = category.id
      ? this.categoryService.update(category.id, category.key, category.displayOrder, category.translations)
      : this.categoryService.create(category.key, category.displayOrder, category.translations);

    action.subscribe(() => {
      this.loadCategories();
      this.editingCategory.set(null);
    });
  }

  deleteCategory(id: string) {
    if (!confirm('Delete category?')) return;
    this.categoryService.delete(id).subscribe(() => this.loadCategories());
  }

  createSkill() {
    if (!this.selectedCategoryId()) return;

    this.editingSkill.set({
      id: '',
      key: '',
      category: this.categories().find(c => c.id === this.selectedCategoryId())!,
      translations: this.emptySkillTranslations()
    } as any);
  }

  saveSkill() {
    const skill = this.editingSkill();
    if (!skill) return;

    const action = skill.id
      ? this.skillService.update(skill)
      : this.skillService.create(this.selectedCategoryId()!, skill.key, skill.translations);

    action.subscribe(() => {
      this.loadSkills();
      this.editingSkill.set(null);
    });
  }

  deleteSkill(id: string) {
    if (!confirm('Delete skill?')) return;
    this.skillService.delete(id).subscribe(() => this.loadSkills());
  }

  addUserSkill(skillId: string, proficiency: number) {
    this.userSkillService.create(skillId, proficiency)
      .subscribe(() => this.loadUserSkills());
  }

  updateUserSkill(id: string, proficiency: number) {
    this.userSkillService.update(id, proficiency)
      .subscribe(() => this.loadUserSkills());
  }

  deleteUserSkill(id: string) {
    this.userSkillService.delete(id)
      .subscribe(() => this.loadUserSkills());
  }


  addLearningSkill(skillId: string, status: LearningStatus, order: number) {
    this.learningSkillService.create(skillId, status, order)
      .subscribe(() => this.loadLearningSkills());
  }

  updateLearningSkill(id: string, status: LearningStatus, order: number) {
    this.learningSkillService.update(id, status, order)
      .subscribe(() => this.loadLearningSkills());
  }

  deleteLearningSkill(id: string) {
    this.learningSkillService.delete(id)
      .subscribe(() => this.loadLearningSkills());
  }

  private emptyCategoryTranslations() {
    return ['en', 'pl', 'ru', 'uk'].map(l => ({
      languageCode: l,
      name: '',
      description: ''
    })) as any;
  }

  private emptySkillTranslations() {
    return ['en', 'pl', 'ru', 'uk'].map(l => ({
      languageCode: l,
      name: ''
    })) as any;
  }

  cancelCategoryEdit() {
    this.editingCategory.set(null);
  }

  cancelSkillEdit() {
    this.editingSkill.set(null);
  }

  isInLearningSkills(skillId: string) {
    return this.learningSkills().some(ls => ls.skill.id === skillId);
  }

  isInUserSkills(skillId: string) {
    return this.userSkills().some(us => us.skill.id === skillId);
  }

  protected readonly LearningStatus = LearningStatus;
  protected readonly Number = Number;
}
