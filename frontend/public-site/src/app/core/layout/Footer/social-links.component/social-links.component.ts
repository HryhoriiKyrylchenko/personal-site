import {Component, computed, inject} from '@angular/core';
import {SiteInfoService} from '../../../services/site-info.service';

@Component({
  selector: 'app-social-links',
  imports: [],
  templateUrl: './social-links.component.html',
  styleUrl: './social-links.component.scss'
})
export class SocialLinksComponent {
  private svc = inject(SiteInfoService);
  socialLinks = computed(() => this.svc.siteInfo()?.socialLinks ?? []);
}
