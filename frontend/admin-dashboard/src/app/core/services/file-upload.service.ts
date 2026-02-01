import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import {environment} from '../../../environments/environment';

export interface FileUploadResponse {
  url: string;
}

export enum UploadFolder {
  Uploads = 'Uploads',
  Projects = 'Projects',
  Blog = 'Blog',
  Resumes = 'Resumes',
  Profile = 'Profile',
  Seo = 'Seo'
}

@Injectable({
  providedIn: 'root'
})
export class FileUploadService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/FileUpload`;

  upload(
    file: File,
    folder: UploadFolder = UploadFolder.Uploads
  ): Observable<string> {
    const formData = new FormData();
    formData.append('file', file);

    const params = new HttpParams().set('folder', folder);

    return this.http
      .post<FileUploadResponse>(this.baseUrl, formData, { params, withCredentials: true })
      .pipe(map(r => r.url));
  }

  delete(fileUrl: string): Observable<void> {
    const params = new HttpParams().set('fileUrl', fileUrl);

    return this.http.delete<void>(this.baseUrl, { params, withCredentials: true });
  }
}
