import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface SkillCategoryTranslationDto {
  id?: string;
  languageCode: string;
  skillCategoryId?: string;
  name: string;
  description: string;
}

export interface SkillCategoryAdminDto {
  id: string;
  key: string;
  displayOrder: number;
  translations: SkillCategoryTranslationDto[];
}

@Injectable({ providedIn: 'root' })
export class SkillCategoryService {
  private http = inject(HttpClient);
  private baseUrl = `${environment.apiUrl}/SkillCategory`;

  getAll(
    keyFilter?: string,
    minDisplayOrder?: number,
    maxDisplayOrder?: number
  ): Observable<SkillCategoryAdminDto[]> {
    let params = new HttpParams();

    if (keyFilter) {
      params = params.set('KeyFilter', keyFilter);
    }
    if (minDisplayOrder !== undefined) {
      params = params.set('MinDisplayOrder', minDisplayOrder);
    }
    if (maxDisplayOrder !== undefined) {
      params = params.set('MaxDisplayOrder', maxDisplayOrder);
    }

    return this.http.get<SkillCategoryAdminDto[]>(this.baseUrl, { params });
  }

  getById(id: string): Observable<SkillCategoryAdminDto> {
    return this.http.get<SkillCategoryAdminDto>(`${this.baseUrl}/${id}`);
  }

  create(
    key: string,
    displayOrder: number,
    translations: SkillCategoryTranslationDto[]
  ): Observable<string> {
    const payload = {
      key,
      displayOrder,
      translations: translations.map(t => ({
        id: '00000000-0000-0000-0000-000000000000',
        languageCode: t.languageCode,
        skillCategoryId: '00000000-0000-0000-0000-000000000000',
        name: t.name,
        description: t.description
      }))
    };

    return this.http.post<string>(this.baseUrl, payload);
  }

  update(
    id: string,
    key: string,
    displayOrder: number,
    translations: SkillCategoryTranslationDto[]
  ) {
    const payload = {
      id,
      key,
      displayOrder,
      translations: translations.map(t => ({
        id: t.id ?? '00000000-0000-0000-0000-000000000000',
        languageCode: t.languageCode,
        skillCategoryId: id,
        name: t.name,
        description: t.description
      }))
    };

    return this.http.put(`${this.baseUrl}/${id}`, payload);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
