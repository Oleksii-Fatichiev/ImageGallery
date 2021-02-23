import { Component, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { equalField } from 'src/app/_helpers/equal-field.validator';
import { AuthService } from 'src/app/_services/auth.service';

import { ModalComponent } from 'src/app/_modals/register-modal';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html'
})
export class RegisterComponent {
  registerForm: FormGroup;
  loading = false;
  submitted = false;
  returnUrl: string;
  showErrorMessage = false;

  @ViewChild(ModalComponent) private modal: ModalComponent;

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService) {
    this.returnUrl = '/login';
  }

  ngOnInit() {
    this.registerForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(100)]],
      checkPassword: ['', [Validators.required, equalField('password')]]
    })
  }

  get f() { return this.registerForm.controls; }

  onSubmit() {
    this.submitted = true;
    if (this.registerForm.invalid) {
      return;
    }
    this.loading = true;
    this.authService.register(this.f.email.value, this.f.password.value, this.f.email.value)
    .subscribe(() => {
      this.loading = false;
      this.modal.open();
    }, (error: any) => {
      error.message === 'User already exists!';
      this.showErrorMessage = true;
      this.loading = false;
    });
  }
}
