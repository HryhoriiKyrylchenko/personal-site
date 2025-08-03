import {Component, HostListener, inject} from '@angular/core';
import {BlogPostDto} from '../../../shared/models/page-dtos';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {NgOptimizedImage} from '@angular/common';
import {MatButton} from '@angular/material/button';
import {ShareLinkComponent} from '../share-link.component/share-link.component';

@Component({
  selector: 'app-blog.detail.component',
  standalone: true,
  imports: [
    NgOptimizedImage,
    MatButton,
    ShareLinkComponent
  ],
  templateUrl: './blog.detail.component.html',
  styleUrl: './blog.detail.component.scss'
})
export class BlogDetailComponent {
  readonly post: BlogPostDto = inject(MAT_DIALOG_DATA);
  private dialogRef = inject(MatDialogRef<BlogDetailComponent>);
  public currentUrl = window.location.href;
  iconSize = '3.0075rem';

  onCloseClick() {
    this.dialogRef.close();
  }

  @HostListener('window:resize')
  onResize(): void {
    const width = window.innerWidth;
    this.iconSize = width >= 640 ? '4rem' : '3.0075rem';
  }
}
