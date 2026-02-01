import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BlogPostService, BlogPostAdminDto } from '../../../core/services/blog-post.service';
import {FileUploadService, UploadFolder} from '../../../core/services/file-upload.service';

@Component({
  selector: 'app-blog-tab',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './blog-tab.component.html',
  styleUrl: './blog-tab.component.scss'
})
export class BlogTabComponent implements OnInit {
  private blogService = inject(BlogPostService);
  private readonly fileUploadService = inject(FileUploadService);

  posts = signal<BlogPostAdminDto[]>([]);
  page = signal(1);
  pageSize = 10;
  totalPages = signal(1);
  slugFilter = signal('');
  isPublishedFilter = signal<boolean | undefined>(undefined);

  editingPost = signal<BlogPostAdminDto | null>(null);

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
    this.loadPosts();
  }

  activeLang = this.editingPost()?.translations[0]?.languageCode ?? 'en';

  openCreate() {
    this.editingPost.set({
      id: '',
      slug: '',
      coverImage: '',
      createdAt: new Date().toISOString(),
      updatedAt: undefined,
      publishedAt: undefined,
      isDeleted: false,
      isPublished: false,
      translations: [
        { languageCode: 'en', title: '', excerpt: '', content: '', metaTitle: '', metaDescription: '', ogImage: '' },
        { languageCode: 'pl', title: '', excerpt: '', content: '', metaTitle: '', metaDescription: '', ogImage: '' },
        { languageCode: 'ru', title: '', excerpt: '', content: '', metaTitle: '', metaDescription: '', ogImage: '' },
        { languageCode: 'uk', title: '', excerpt: '', content: '', metaTitle: '', metaDescription: '', ogImage: '' }
      ],
      tags: []
    });

    this.activeLang = 'en';
  }

  editPost(post: BlogPostAdminDto) {
    this.editingPost.set(JSON.parse(JSON.stringify(post)));
  }

  cancelEdit() {
    this.editingPost.set(null);
  }

  savePost() {
    const post = this.editingPost();
    if (!post) return;

    const request = post.id
      ? this.blogService.update(post.id, post)
      : this.blogService.create(post);

    request.subscribe(() => {
      this.loadPosts();
      this.cancelEdit();
    });
  }

  addTag(name: string) {
    if (!name.trim()) return;
    this.editingPost.update(p => {
      if (!p) return p;
      if (!p.tags.find(t => t.name === name)) p.tags.push({ name } as any);
      return p;
    });
  }

  removeTag(name: string) {
    this.editingPost.update(p => {
      if (!p) return p;
      p.tags = p.tags.filter(t => t.name !== name);
      return p;
    });
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

  uploadCoverImage(event: Event) {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (!file) return;

    this.fileUploadService
      .upload(file, UploadFolder.Blog)
      .subscribe(url => {
        this.editingPost.update(p => {
          if (!p) return p;
          p.coverImage = url;
          return p;
        });
      });
  }

  deleteCoverImage() {
    const url = this.editingPost()?.coverImage;
    if (!url) return;

    this.fileUploadService.delete(url).subscribe(() => {
      this.editingPost.update(p => {
        if (!p) return p;
        p.coverImage = '';
        return p;
      });
    });
  }

  uploadOgImage(event: Event, languageCode: string) {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (!file) return;

    this.fileUploadService
      .upload(file, UploadFolder.Seo)
      .subscribe(url => {
        this.editingPost.update(p => {
          if (!p) return p;
          const tr = p.translations.find(t => t.languageCode === languageCode);
          if (tr) tr.ogImage = url;
          return p;
        });
      });
  }

  deleteOgImage(languageCode: string) {
    const tr = this.editingPost()?.translations.find(t => t.languageCode === languageCode);
    if (!tr?.ogImage) return;

    this.fileUploadService.delete(tr.ogImage).subscribe(() => {
      this.editingPost.update(p => {
        if (!p) return p;
        const t = p.translations.find(x => x.languageCode === languageCode);
        if (t) t.ogImage = '';
        return p;
      });
    });
  }
}
