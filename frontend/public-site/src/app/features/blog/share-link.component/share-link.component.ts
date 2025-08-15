import {Component, inject, Input} from '@angular/core';
import {MatSnackBar} from '@angular/material/snack-bar';
import { Clipboard } from '@angular/cdk/clipboard';
import {TranslocoPipe, TranslocoService} from '@ngneat/transloco';
import {MatIconButton} from '@angular/material/button';
import {MatTooltip} from '@angular/material/tooltip';
import {AnalyticsService} from '../../../core/services/analytics-service';

@Component({
  selector: 'app-share-links',
  standalone: true,
  imports: [
    MatIconButton,
    MatTooltip,
    TranslocoPipe
  ],
  templateUrl: './share-link.component.html',
  styleUrl: './share-link.component.scss'
})
export class ShareLinkComponent {
  @Input() url!: string;
  @Input() label?: string;
  private _iconSize = '3.5rem';

  private clipboard = inject(Clipboard);
  private snackBar = inject(MatSnackBar);
  private transloco = inject(TranslocoService)

  private analytics = inject(AnalyticsService);

  onShareLinkedIn(): void {
    const shareUrl = `https://www.linkedin.com/sharing/share-offsite/?url=${encodeURIComponent(this.url)}`;
    window.open(shareUrl, '_blank', 'noopener,noreferrer');

    this.analytics.trackEvent({
      eventType: "share_link_click",
      pageSlug: "",
      additionalDataJson: "{}"
    });
  }

  onShareFacebook(): void {
    const shareUrl = `https://www.facebook.com/sharer/sharer.php?u=${encodeURIComponent(this.url)}`;
    window.open(shareUrl, '_blank', 'noopener,noreferrer');

    this.analytics.trackEvent({
      eventType: "share_link_click",
      pageSlug: "",
      additionalDataJson: "{}"
    });
  }

  async onCopyLink(): Promise<void> {
    const textToCopy = this.label
      ? `${this.label} – ${this.url}`
      : this.url;
    this.clipboard.copy(textToCopy);
    this.snackBar.open(
      this.transloco.translate('button.copied'),
      undefined,
      { duration: 2000 });

    this.analytics.trackEvent({
      eventType: "share_link_copy",
      pageSlug: "",
      additionalDataJson: "{}"
    }).subscribe();
  }


  @Input()
  set iconSize(size: string) {
    this._iconSize = size;
  }
  get iconSize() {
    return this._iconSize;
  }
}
