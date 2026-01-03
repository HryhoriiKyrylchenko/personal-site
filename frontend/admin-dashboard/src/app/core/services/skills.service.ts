import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {SkillCategoryAdminDto} from './skill-category.service';

export interface SkillTranslationDto {
  id: string;
  languageCode: string;
  skillId: string;
  name: string;
  description: string;
}

export interface SkillAdminDto {
  id: string;
  key: string;
  category: SkillCategoryAdminDto;
  translations: SkillTranslationDto[];
}

export interface CreateSkillTranslation {
  languageCode: string;
  name: string;
  description: string;
}

@Injectable({ providedIn: 'root' })
export class SkillService {
  private http = inject(HttpClient);
  private baseUrl = `${environment.apiUrl}/Skill`;

  getAll(categoryId?: string, keyFilter?: string)
  : Observable<SkillAdminDto[]> {
    let params = new HttpParams();

    if (categoryId) params = params.set('CategoryId', categoryId);
    if (keyFilter) params = params.set('KeyFilter', keyFilter);

    return this.http.get<SkillAdminDto[]>(this.baseUrl, { params });
  }

  getById(id: string): Observable<SkillAdminDto> {
    return this.http.get<SkillAdminDto>(`${this.baseUrl}/${id}`);
  }

  create(categoryId: string, key: string, translations: CreateSkillTranslation[]): Observable<string> {
    const payload = {
      CategoryId: categoryId,
      Key: key,
      Translations: translations
    };
    return this.http.post<string>(this.baseUrl, payload);
  }

  update(skill: SkillAdminDto) {
    const payload = {
      Id: skill.id,
      CategoryId: skill.category.id,
      Key: skill.key,
      Translations: skill.translations
    };
    return this.http.put(`${this.baseUrl}/${skill.id}`, payload);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
