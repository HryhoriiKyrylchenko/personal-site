import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface ResumeDto {
  id: string;
  fileUrl: string;
  fileName: string;
  uploadedAt: string;
  isActive: boolean;
}

export interface PaginatedResult<T> {
  value: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

@Injectable({ providedIn: 'root' })
export class ResumeService {
  private http = inject(HttpClient);
  private baseUrl = `${environment.apiUrl}/Resume`;
  getAll(pageNumber = 1, pageSize = 10): Observable<PaginatedResult<ResumeDto>> {
    let params = new HttpParams()
      .set('PageNumber', pageNumber)
      .set('PageSize', pageSize);

    return this.http.get<PaginatedResult<ResumeDto>>(this.baseUrl, { params, withCredentials: true });
  }

  getById(id: string): Observable<ResumeDto> {
    return this.http.get<ResumeDto>(`${this.baseUrl}/${id}`, { withCredentials: true });
  }

  create(fileUrl: string, fileName: string, isActive: boolean): Observable<string> {
    const payload = { fileUrl, fileName, isActive };
    return this.http.post<string>(this.baseUrl, payload, { withCredentials: true });
  }

  update(resume: ResumeDto) {
    const payload = {
      id: resume.id,
      fileUrl: resume.fileUrl,
      fileName: resume.fileName,
      isActive: resume.isActive
    };
    return this.http.put(`${this.baseUrl}/${resume.id}`, payload, { withCredentials: true });
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`, { withCredentials: true });
  }
}
