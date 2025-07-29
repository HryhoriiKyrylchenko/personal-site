import {Directive, ElementRef, EventEmitter, Output, OnDestroy, AfterViewInit, inject} from '@angular/core';
import { fromEvent, Subscription } from 'rxjs';
import { filter } from 'rxjs/operators';
import { DOCUMENT } from '@angular/common';

@Directive({
  selector: '[appClickOutside]'
})
export class ClickOutsideDirective implements AfterViewInit, OnDestroy {
  @Output() clickOutside = new EventEmitter<void>();
  private sub?: Subscription;
  private elementRef = inject(ElementRef);
  private document = inject(DOCUMENT);

  ngAfterViewInit() {
    this.sub = fromEvent<MouseEvent>(this.document, 'click')
      .pipe(
        filter(event => !this.elementRef.nativeElement.contains(event.target as Node))
      )
      .subscribe(() => this.clickOutside.emit());
  }

  ngOnDestroy() {
    this.sub?.unsubscribe();
  }
}
