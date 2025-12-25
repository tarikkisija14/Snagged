import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth-service/AuthService';
import{MatSnackBar} from '@angular/material/snack-bar';


@Component({
  selector: 'app-register',
  standalone: false,
  templateUrl: './register.html',
  styleUrls: ['./register.scss'],
})
export class Register {
  registerForm: FormGroup;
  passwordMismatch = false;
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.registerForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.maxLength(50)]],
      lastName: ['', [Validators.required, Validators.maxLength(50)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]],
      agreeToTerms: [false, [Validators.requiredTrue]],
    });
  }

  onSubmit() {
    if (this.registerForm.invalid) {
      this.markFormGroupTouched(this.registerForm);
      return;
    }

    const {
      firstName,
      lastName,
      email,
      password,
      confirmPassword,
    } = this.registerForm.value;

    if (password !== confirmPassword) {
      this.passwordMismatch = true;
      return;
    }

    this.passwordMismatch = false;
    this.errorMessage = '';

    this.authService
      .register({
        firstName,
        lastName,
        email,
        password,
      })
      .subscribe({
        next: (response) => {
          localStorage.setItem('token', response.token);
          this.snackBar.open('Account created!', 'OK', {
            duration: 2000,
          });
          this.router.navigate(['/']);
        },
        error: (err) => {
          this.errorMessage =
            err.error?.message || 'Registration failed. Please try again.';
        },
      });
  }

  private markFormGroupTouched(formGroup: FormGroup) {
    Object.values(formGroup.controls).forEach((control) => {
      control.markAsTouched();
    });
  }
}
