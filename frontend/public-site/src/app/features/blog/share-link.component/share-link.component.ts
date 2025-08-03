import {Component, inject, Input} from '@angular/core';
import {MatSnackBar} from '@angular/material/snack-bar';

@Component({
  selector: 'app-share-links',
  imports: [],
  templateUrl: './share-link.component.html',
  styleUrl: './share-link.component.scss'
})
export class ShareLinkComponent {
  @Input() url!: string;
  @Input() label?: string;
  private _iconSize = '3.5rem';

  private clipboard = inject(Clipboard);
  private snackBar = inject(MatSnackBar);

  onShareLinkedIn(): void {
    const shareUrl = `https://www.linkedin.com/sharing/share-offsite/?url=${encodeURIComponent(this.url)}`;
    window.open(shareUrl, '_blank', 'noopener,noreferrer');
  }

  onShareFacebook(): void {
    const shareUrl = `https://www.facebook.com/sharer/sharer.php?u=${encodeURIComponent(this.url)}`;
    window.open(shareUrl, '_blank', 'noopener,noreferrer');
  }

  async onCopyLink(): Promise<void> {
    const textToCopy = this.label
      ? `${this.label} – ${this.url}`
      : this.url;
    await this.clipboard.writeText(textToCopy);
    this.snackBar.open(`Link copied!`, undefined, { duration: 2000 });
  }


  @Input()
  set iconSize(size: string) {
    this._iconSize = size;
  }
  get iconSize() {
    return this._iconSize;
  }
}
