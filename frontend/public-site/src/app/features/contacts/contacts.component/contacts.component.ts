import {Component, HostListener, inject, ViewChild} from '@angular/core';
import {PagesApiService} from '../../../core/services/pages-api.service';
import {TranslocoPipe} from '@ngneat/transloco';
import {SocialLinksComponent} from '../../../core/layout/Footer/social-links.component/social-links.component';
import {AutoResizeDirective} from './auto-resize.directive';
import {AsyncPipe} from '@angular/common';
import {FormBuilder, FormGroup, FormGroupDirective, ReactiveFormsModule, Validators} from '@angular/forms';
import {ContactService, SendContactMessageCommand} from '../../../core/services/contact-api.service';
import {ClickOutsideDirective} from '../../../shared/directives/click-outside.directive';

@Component({
  selector: 'app-contacts',
  templateUrl: './contacts.component.html',
  styleUrl: './contacts.component.scss',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    TranslocoPipe,
    SocialLinksComponent,
    AutoResizeDirective,
    AsyncPipe,
    ClickOutsideDirective
  ]
})
export class ContactsComponent {
  iconSize = '5.875rem';
  readonly page$ = inject(PagesApiService).contactsPage$;
  private fb = inject(FormBuilder);
  private contactService = inject(ContactService);
  showModal = false;
  modalType: 'success' | 'error' | null = null;

  contactForm: FormGroup;
  submitted = false;

  @ViewChild('formDirective') formDirective!: FormGroupDirective;

  constructor() {
    this.contactForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      subject: ['', Validators.required],
      message: ['', Validators.required]
    });
  }

  onSubmit() {
    this.submitted = true;
    this.contactForm.markAllAsTouched();

    if (this.contactForm.invalid) return;

    const command: SendContactMessageCommand = this.contactForm.value;

    this.contactService.sendMessage(command).subscribe({
      next: () => {
        this.formDirective.resetForm();
        this.contactForm.reset();

        Object.values(this.contactForm.controls).forEach(c => {
          c.setErrors(null);
          c.markAsPristine();
          c.markAsUntouched();
        });

        this.submitted = false;

        this.modalType = 'success';
        this.showModal = true;
      },
    error: () => {
      this.submitted = false;
    }});
  }

  closeModal() {
    this.showModal = false;
    this.modalType = null;
  }

  @HostListener('window:resize')
  onResize(): void {
    const width = window.innerWidth;
    this.iconSize = width >= 640 ? '7.8125rem' : '5.875rem';
  }
}
