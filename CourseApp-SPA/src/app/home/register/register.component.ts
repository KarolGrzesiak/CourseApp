import { Component, OnInit, Output, EventEmitter, HostListener } from '@angular/core';

import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap';

import { Router } from '@angular/router';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { User } from 'src/app/_models/user';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();

  user: User;
  registerForm: FormGroup;
  bsConfig: Partial<BsDatepickerConfig>;

  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (this.registerForm.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(
    private authService: AuthService,
    private alertify: AlertifyService,
    private formBuilder: FormBuilder,
    private router: Router
  ) {}

  ngOnInit() {
    this.bsConfig = {
      containerClass: 'theme-green'
    };
    this.createRegisterForm();
  }

  createRegisterForm() {
    this.registerForm = this.formBuilder.group(
      {
        gender: ['male'],
        username: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        knownAs: ['', Validators.required],
        dateOfBirth: [null, Validators.required],
        city: ['', Validators.required],
        country: ['', Validators.required],
        password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
        confirmPassword: ['', Validators.required]
      },
      {
        validator: this.passwordMatchValidator
      }
    );
  }

  passwordMatchValidator(registerForm: FormGroup) {
    return registerForm.get('password').value === registerForm.get('confirmPassword').value
      ? null
      : { mismatch: true };
  }

  register() {
    if (this.registerForm.valid) {
      this.user = Object.assign({}, this.registerForm.value);
      this.authService.register(this.user).subscribe(
        () => {
          this.alertify.success('Registration successful');
        },
        error => {
          this.alertify.error(error);
        },
        () => {
          this.authService.login(this.user).subscribe(() => {
            this.router.navigate(['/members']);
          });
        }
      );
    }
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
