import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {SkillAdminDto} from './skills.service';

export interface UserSkillAdminDto {
  id: string;
  skill: SkillAdminDto;
  proficiency: number;
  createdAt: string;
  updatedAt?: string;
}

@Injectable({ providedIn: 'root' })
export class UserSkillService {
  private http = inject(HttpClient);
  private baseUrl = `${environment.apiUrl}/UserSkill`;

  getAll(filter?: {
    skillId?: string;
    minProficiency?: number;
    maxProficiency?: number;
  }): Observable<UserSkillAdminDto[]> {
    let params = new HttpParams();

    if (filter?.skillId)
      params = params.set('SkillId', filter.skillId);

    if (filter?.minProficiency !== undefined)
      params = params.set('MinProficiency', filter.minProficiency);

    if (filter?.maxProficiency !== undefined)
      params = params.set('MaxProficiency', filter.maxProficiency);

    return this.http.get<UserSkillAdminDto[]>(this.baseUrl, { params, withCredentials: true });
  }

  getById(id: string): Observable<UserSkillAdminDto> {
    return this.http.get<UserSkillAdminDto>(`${this.baseUrl}/${id}`, { withCredentials: true });
  }

  create(skillId: string, proficiency: number): Observable<string> {
    const payload = {
      skillId,
      proficiency
    };

    return this.http.post<string>(this.baseUrl, payload, { withCredentials: true });
  }

  update(id: string, proficiency: number): Observable<void> {
    const payload = {
      id,
      proficiency
    };

    return this.http.put<void>(`${this.baseUrl}/${id}`, payload, { withCredentials: true });
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`, { withCredentials: true });
  }
}
