import { Component, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {MAT_DIALOG_DATA, MatDialog} from '@angular/material/dialog';
import {BlogDetailComponent} from '../blog.detail.component/blog.detail.component';
import {BlogPostDto} from '../../../shared/models/page-dtos';

@Component({
  selector: 'app-blog.detail.modal.launcher.component',
  standalone: true,
  imports: [],
  templateUrl: './blog.detail.modal.launcher.component.html',
  styleUrl: './blog.detail.modal.launcher.component.scss'
})
export class BlogDetailModalLauncherComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly dialog = inject(MatDialog);
  public post: BlogPostDto = inject(MAT_DIALOG_DATA);

  constructor() {
    this.openDialog();
  }

  openDialog() {
    const dialogRef = this.dialog.open(BlogDetailComponent, {
      data: this.post
    });

    dialogRef.afterClosed().subscribe(() => {
      this.router.navigate(['/blog']);
    });
  }
}
