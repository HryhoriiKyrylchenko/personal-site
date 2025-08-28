import { Routes } from '@angular/router';
import {LayoutComponent} from './core/layout/Layout/layout.component/layout.component';
import {ErrorComponent} from './features/error/error.component/error.component';

export const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      { path: '**', component: ErrorComponent, data: { title: 'Error'} }
    ]
  },
  { path: '**', redirectTo: '' }
];
