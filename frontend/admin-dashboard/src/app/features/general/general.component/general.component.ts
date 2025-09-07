import { Component } from '@angular/core';
import {InfoTabComponent} from '../info-tab.component/info-tab.component';
import {PagesTabComponent} from '../pages-tab.component/pages-tab.component';
import {SkillsTabComponent} from '../skills-tab.component/skills-tab.component';

@Component({
  selector: 'app-general',
  imports: [
    InfoTabComponent,
    PagesTabComponent,
    SkillsTabComponent
  ],
  templateUrl: './general.component.html',
  styleUrl: './general.component.scss'
})
export class GeneralComponent {
  selectedTab = 0;

  tabs = [
    { label: 'Info' },
    { label: 'Pages' },
    { label: 'Skills' }
  ];
}
