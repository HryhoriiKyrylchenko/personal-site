import { Routes } from '@angular/router';
import {AboutComponent} from './features/about/about.component/about.component';
import {BlogComponent} from './features/blog/blog.component/blog.component';
import {ContactsComponent} from './features/contacts/contacts.component/contacts.component';
import {HomeComponent} from './features/home/home.component/home.component';
import {PortfolioComponent} from './features/portfolio/portfolio.component/portfolio.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'about', component: AboutComponent, data: { title: 'About'} },
  { path: 'portfolio', component: PortfolioComponent, data: { title: 'Portfolio'} },
  { path: 'blog', component: BlogComponent, data: { title: 'Blog'} },
  { path: 'contacts', component: ContactsComponent, data: { title: 'Contacts'} },
  { path: '**', redirectTo: '' }
];
