import { Component } from '@angular/core';

@Component({
  selector: 'app-social-links',
  imports: [],
  templateUrl: './social-links.component.html',
  styleUrl: './social-links.component.scss'
})
export class SocialLinksComponent {
  socialLinks = [
    { id: '1', platform: 'LinkedIn', url: 'https://www.linkedin.com/in/your-profile' },
    { id: '2', platform: 'GitHub', url: 'https://github.com/your-profile' },
    { id: '3', platform: 'Facebook', url: 'https://facebook.com/your-profile' }
  ];

  // private siteInfoSvc = inject(SiteInfoService);
  // socialLinks$!: Observable<SocialMediaLinkDto[]>;
  //
  // constructor() {
  //   this.socialLinks$ = this.siteInfoSvc.socialMediaLinks$;
  // }
}
