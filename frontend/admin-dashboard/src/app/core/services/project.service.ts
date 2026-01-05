import { Injectable, inject } from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {map, Observable} from 'rxjs';
import { environment } from '../../../environments/environment';
import {SkillAdminDto} from './skills.service';

export interface ProjectTranslationDto {
  id?: string;
  languageCode: string;
  projectId?: string;
  title: string;
  shortDescription: string;
  descriptionSections: string;
  metaTitle: string;
  metaDescription: string;
  ogImage: string;
}

export interface ProjectSkillAdminDto {
  id?: string;
  projectId?: string;
  skill: SkillAdminDto;
}

export interface ProjectAdminDto {
  id: string;
  slug: string;
  coverImage: string;
  demoUrl: string;
  repoUrl: string;
  createdAt: string;
  updatedAt?: string;
  translations: ProjectTranslationDto[];
  skills: ProjectSkillAdminDto[];
}

export interface PaginatedResult<T> {
  value: T[];
  isSuccess: boolean;
  error?: string;
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

@Injectable({ providedIn: 'root' })
export class ProjectService {
  private http = inject(HttpClient);
  private baseUrl = `${environment.apiUrl}/Project`;

  getAll(page = 1, pageSize = 10, slugFilter?: string)
    : Observable<PaginatedResult<ProjectAdminDto>> {
    let params = new HttpParams()
      .set('Page', page)
      .set('PageSize', pageSize);

    if (slugFilter) params = params.set('SlugFilter', slugFilter);
    return this.http.get<PaginatedResult<ProjectAdminDto>>(this.baseUrl, { params }).pipe(
      map(res => {
        res.value.forEach(project =>
          project.translations.forEach(t =>
            t.descriptionSections = JSON.stringify(t.descriptionSections)
          )
        );
        return res;
      })
    );
  }

  getById(id: string): Observable<ProjectAdminDto> {
    return this.http.get<ProjectAdminDto>(`${this.baseUrl}/${id}`);
  }

  create(project: ProjectAdminDto): Observable<string> {
    const payload = {
      slug: project.slug,
      coverImage: project.coverImage,
      demoUrl: project.demoUrl,
      repoUrl: project.repoUrl,
      translations: project.translations.map(t => ({
        id: '00000000-0000-0000-0000-000000000000',
        languageCode: t.languageCode,
        projectId: '00000000-0000-0000-0000-000000000000',
        title: t.title,
        shortDescription: t.shortDescription,
        descriptionSections: JSON.parse(t.descriptionSections),
        metaTitle: t.metaTitle,
        metaDescription: t.metaDescription,
        ogImage: t.ogImage
      })),
      skillIds: project.skills.map(s => s.skill.id)
    };

    return this.http.post<string>(this.baseUrl, payload);
  }

  update(id: string, project: ProjectAdminDto) {
    const payload = {
      id,
      slug: project.slug,
      coverImage: project.coverImage,
      demoUrl: project.demoUrl,
      repoUrl: project.repoUrl,
      translations: project.translations.map(t => ({
        id: t.id ?? '00000000-0000-0000-0000-000000000000',
        languageCode: t.languageCode,
        projectId: id,
        title: t.title,
        shortDescription: t.shortDescription,
        descriptionSections: JSON.parse(t.descriptionSections),
        metaTitle: t.metaTitle,
        metaDescription: t.metaDescription,
        ogImage: t.ogImage
      })),
      skillIds: project.skills.map(s => s.skill.id)
    };

    return this.http.put(`${this.baseUrl}/${id}`, payload);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
