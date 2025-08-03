import { Component, inject } from '@angular/core';
import { PagesApiService } from '../../../core/services/pages-api.service';
import {AsyncPipe, NgStyle} from '@angular/common';
import {Router} from '@angular/router';
import {TranslocoPipe} from '@ngneat/transloco';
import {map} from 'rxjs';
import {BlogPostDto} from '../../../shared/models/page-dtos';

@Component({
  selector: 'app-blog',
  standalone: true,
  imports: [
    TranslocoPipe,
    AsyncPipe,
    NgStyle
  ],
  templateUrl: './blog.component.html',
  styleUrl: './blog.component.scss'
})
export class BlogComponent {
  readonly page$ = inject(PagesApiService).blogPage$;
  readonly posts$ = this.page$.pipe(map(page => page.blogPosts));
  private router = inject(Router);

  onReadClick(slug: string) {
    void this.router.navigate([`/blog/${slug}`]);
  }

  onShareClick(post: BlogPostDto) {
    return post; // Real sharing logic goes here
  }
}
