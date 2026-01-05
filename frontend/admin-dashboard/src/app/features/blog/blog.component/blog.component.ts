import { Component } from '@angular/core';
import {BlogTabComponent} from '../blog-tab.component/blog-tab.component';

@Component({
  selector: 'app-blog',
  standalone: true,
  imports: [
    BlogTabComponent
  ],
  templateUrl: './blog.component.html',
  styleUrl: './blog.component.scss'
})
export class BlogComponent {
  selectedTab = 0;

  tabs = [
    { label: 'Blog' }
  ];
}
