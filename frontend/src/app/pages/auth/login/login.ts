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
    console.log('Login attempt with:', this.email, this.password);
    this.authService.login({email: this.email, password: this.password}).subscribe({
      next: res => {
        localStorage.setItem('token', res.token);
        const userId = this.authService.getUserId();
        console.log('UserId after login:', userId);




        this.snackBar.open('Login successful', 'OK', {duration: 2000});
        this.router.navigate(['/']);
      },
      error: err => {
        console.error('Login error:', err);
        this.snackBar.open('Login failed', 'OK', {duration: 2000});
      }
    });
  }
}
