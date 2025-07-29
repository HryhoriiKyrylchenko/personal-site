import {Directive, ElementRef, HostListener, inject, OnInit} from '@angular/core';

@Directive({ selector: '[appAutoResize]' })
export class AutoResizeDirective implements OnInit {
  private ta: HTMLTextAreaElement;
  private el = inject(ElementRef<HTMLTextAreaElement>);

  constructor() {
    this.ta = this.el.nativeElement;
  }

  ngOnInit(): void {
    setTimeout(() => this.adjust(), 0);
  }

  @HostListener('input')
  onInput(): void {
    this.adjust();
  }

  private adjust(): void {
    this.ta.style.height = 'auto';
    this.ta.style.height = this.ta.scrollHeight + 'px';
  }
}
