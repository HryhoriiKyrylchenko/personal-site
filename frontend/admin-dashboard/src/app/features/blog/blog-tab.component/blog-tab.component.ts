import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BlogPostService, BlogPostAdminDto } from '../../../core/services/blog-post.service';

@Component({
  selector: 'app-blog-tab',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './blog-tab.component.html',
  styleUrl: './blog-tab.component.scss'
})
export class BlogTabComponent implements OnInit {
  private blogService = inject(BlogPostService);

  posts = signal<BlogPostAdminDto[]>([]);
  page = signal(1);
  pageSize = 10;
  totalPages = signal(1);
  slugFilter = signal('');
  isPublishedFilter = signal<boolean | undefined>(undefined);

  ngOnInit() {
    this.loadPosts();
  }

  loadPosts() {
    this.blogService.getAll(this.page(), this.pageSize, this.slugFilter(), this.isPublishedFilter())
      .subscribe(res => {
        if (res.isSuccess) {
          this.posts.set(res.value);
          this.totalPages.set(res.totalPages);
        }
      });
  }

  deletePost(id: string) {
    if (!confirm('Delete this post?')) return;
    this.blogService.delete(id).subscribe(() => this.loadPosts());
  }

  publishPost(id: string, isPublished: boolean) {
    this.blogService.publish(id, !isPublished, new Date().toISOString())
      .subscribe(() => this.loadPosts());
  }

  prevPage() {
    if (this.page() > 1) {
      this.page.update(p => p - 1);
      this.loadPosts();
    }
  }

  nextPage() {
    if (this.page() < this.totalPages()) {
      this.page.update(p => p + 1);
      this.loadPosts();
    }
  }

  get slugFilterValue() {
    return this.slugFilter();
  }
  set slugFilterValue(val: string) {
    this.slugFilter.set(val);
  }

  get isPublishedFilterValue() {
    return this.isPublishedFilter();
  }
  set isPublishedFilterValue(val: boolean | undefined) {
    this.isPublishedFilter.set(val);
  }
}
