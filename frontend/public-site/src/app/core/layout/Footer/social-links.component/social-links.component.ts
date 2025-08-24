import {ChangeDetectionStrategy, Component, computed, inject, Input} from '@angular/core';
import {SiteInfoService} from '../../../services/site-info.service';

@Component({
  selector: 'app-social-links',
  imports: [],
  templateUrl: './social-links.component.html',
  styleUrl: './social-links.component.scss',
  changeDetection: ChangeDetectionStrategy.Default
})
export class SocialLinksComponent {
  private _iconSize = '3.5rem';

  @Input()
  set iconSize(size: string) {
    this._iconSize = size;
  }
  get iconSize() {
    return this._iconSize;
  }

  private svc = inject(SiteInfoService);
  socialLinks = computed(() => this.svc.siteInfo()?.socialLinks ?? []);
}
