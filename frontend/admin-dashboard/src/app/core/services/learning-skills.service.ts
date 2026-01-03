import { Injectable, inject } from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { SkillAdminDto } from './skills.service';

export enum LearningStatus {
  Planned = 0,
  InProgress = 1,
  Practicing = 2
}

export interface LearningSkillAdminDto {
  id: string;
  skill: SkillAdminDto;
  learningStatus: LearningStatus;
  displayOrder: number;
}

@Injectable({ providedIn: 'root' })
export class LearningSkillService {
  private http = inject(HttpClient);
  private baseUrl = `${environment.apiUrl}/LearningSkills`;

  getAll(filter?: {
    learningStatus?: LearningStatus;
  }): Observable<LearningSkillAdminDto[]> {
    let params = new HttpParams();

    if (filter?.learningStatus !== undefined)
      params = params.set('LearningStatus', filter.learningStatus);

    return this.http.get<LearningSkillAdminDto[]>(this.baseUrl, { params });
  }

  getById(id: string): Observable<LearningSkillAdminDto> {
    return this.http.get<LearningSkillAdminDto>(`${this.baseUrl}/${id}`);
  }

  create(
    skillId: string,
    learningStatus: LearningStatus,
    displayOrder: number
  ): Observable<string> {
    const payload = {
      skillId,
      learningStatus,
      displayOrder
    };

    return this.http.post<string>(this.baseUrl, payload);
  }

  update(
    id: string,
    learningStatus: LearningStatus,
    displayOrder: number
  ): Observable<void> {
    const payload = {
      id,
      learningStatus,
      displayOrder
    };

    return this.http.put<void>(`${this.baseUrl}/${id}`, payload);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
