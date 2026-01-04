import { Component } from '@angular/core';
import { AuthService } from '../../../core/services/auth-service/AuthService';
import {Router} from '@angular/router';
import{MatSnackBar} from '@angular/material/snack-bar';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.html',
  styleUrls: ['./login.scss']
})
export class Login {
  email = '';
  password = '';
  errorMessage = '';
  successMessage = '';

  constructor(private authService: AuthService, private router: Router, private snackBar: MatSnackBar) {
  }

  login() {
    this.authService.login({email: this.email, password: this.password}).subscribe({
      next: () => {
        this.snackBar.open('Login successful', 'OK', {duration: 2000});
        this.router.navigate(['/']);
      },
      error: () => this.snackBar.open('Login failed', 'OK', {duration: 2000})
    });
  }
}
