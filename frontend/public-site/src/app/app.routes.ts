import { Routes } from '@angular/router';
import {AboutComponent} from './features/about/about.component/about.component';
import {BlogComponent} from './features/blog/blog.component/blog.component';
import {ContactsComponent} from './features/contacts/contacts.component/contacts.component';
import {HomeComponent} from './features/home/home.component/home.component';
import {PortfolioComponent} from './features/portfolio/portfolio.component/portfolio.component';
import {LayoutComponent} from './core/layout/Layout/layout.component/layout.component';
import {PrivacyComponent} from './features/privacy/privacy.component/privacy.component';
import {CookiesComponent} from './features/cookies/cookies.component/cookies.component';
import {ErrorComponent} from './features/error/error.component/error.component';

export const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'about', component: AboutComponent, data: { title: 'About'} },
      { path: 'portfolio', component: PortfolioComponent, data: { title: 'Portfolio'} },
      { path: 'blog', component: BlogComponent, data: { title: 'Blog'} },
      { path: 'contacts', component: ContactsComponent, data: { title: 'Contacts'} },
      { path: 'privacy', component: PrivacyComponent, data: { title: 'Privacy'} },
      { path: 'cookies', component: CookiesComponent, data: { title: 'Cookies'} },
      { path: '**', component: ErrorComponent, data: { title: 'Error'} }
    ]
  },
  { path: '**', redirectTo: '' }
];
