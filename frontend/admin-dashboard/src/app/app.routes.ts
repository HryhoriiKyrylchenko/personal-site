import { Routes } from '@angular/router';
import {LayoutComponent} from './core/layout/Layout/layout.component/layout.component';
import {ErrorComponent} from './features/error/error.component/error.component';
import {AnalyticsComponent} from './features/analytics/analytics.component/analytics.component';
import {BlogComponent} from './features/blog/blog.component/blog.component';
import {PortfolioComponent} from './features/portfolio/portfolio.component/portfolio.component';
import {GeneralComponent} from './features/general/general.component/general.component';

export const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      { path: '', component: AnalyticsComponent, pathMatch: 'full' },
      { path: 'blog', component: BlogComponent, data: { title: 'Blog'} },
      { path: 'portfolio', component: PortfolioComponent, data: { title: 'Portfolio'} },
      { path: 'general', component: GeneralComponent, data: { title: 'General'} },
      { path: '**', component: ErrorComponent, data: { title: 'Error'} }
    ]
  },
  { path: '**', redirectTo: '' }
];
