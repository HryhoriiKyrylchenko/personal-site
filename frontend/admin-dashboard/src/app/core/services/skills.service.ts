import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface SkillTranslationDto {
  id: string;
  languageCode: string;
  skillId: string;
  name: string;
  description: string;
}

export interface SkillCategoryTranslationDto {
  id: string;
  languageCode: string;
  skillCategoryId: string;
  name: string;
  description: string;
}

export interface SkillCategoryAdminDto {
  id: string;
  key: string;
  displayOrder: number;
  translations: SkillCategoryTranslationDto[];
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

export interface CreateSkillRequest {
  categoryId: string;
  key: string;
  translations: CreateSkillTranslation[];
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

  create(request: CreateSkillRequest): Observable<string> {
    return this.http.post<string>(this.baseUrl, request);
  }

  update(id: string, request: CreateSkillRequest): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, {
      id,
      ...request
    });
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  mapToCreateRequest(skill: SkillAdminDto): CreateSkillRequest {
    return {
      categoryId: skill.category.id,
      key: skill.key,
      translations: skill.translations.map(t => ({
        languageCode: t.languageCode,
        name: t.name,
        description: t.description
      }))
    };
  }
}
