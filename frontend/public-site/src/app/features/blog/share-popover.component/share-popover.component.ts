import {
  Component,
  Input,
  ViewChild,
  ElementRef,
  HostListener,
  inject
} from '@angular/core';
import { Clipboard } from '@angular/cdk/clipboard';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-share-popover',
  standalone: true,
  templateUrl: './share-popover.component.html',
  styleUrls: ['./share-popover.component.scss'],
})
export class SharePopoverComponent {
  @Input() url!: string;
  @Input() label?: string;

  @ViewChild('triggerBtn', { static: true, read: ElementRef })
  private triggerBtn?: ElementRef<HTMLElement>;

  open = false;

  private clipboard = inject(Clipboard);
  private snackBar = inject(MatSnackBar);

  onToggle(evt: MouseEvent) {
    evt.stopPropagation();
    this.open = !this.open;
  }

  onShareLinkedIn() {
    const share = `https://www.linkedin.com/sharing/share-offsite/?url=${encodeURIComponent(this.url)}`;
    window.open(share, '_blank', 'noopener,noreferrer');
    this.open = false;
  }

  onShareFacebook() {
    const share = `https://www.facebook.com/sharer/sharer.php?u=${encodeURIComponent(this.url)}`;
    window.open(share, '_blank', 'noopener,noreferrer');
    this.open = false;
  }

  async onCopyLink() {
    this.clipboard.copy(this.url);
    this.snackBar.open('Link copied!', undefined, { duration: 2000 });
    this.open = false;
  }

  @HostListener('document:click', ['$event.target'])
  public onClickOutside(target: EventTarget | null): void {
    if (!this.open || !this.triggerBtn) return;
    if (target instanceof HTMLElement && this.triggerBtn.nativeElement.contains(target)) {
      return;
    }
    this.open = false;
  }

  @HostListener('document:keydown.escape')
  public onEscape(): void {
    this.open = false;
  }
}
