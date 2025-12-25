import { Component } from '@angular/core';
import {AuthService} from '../../core/services/auth-service/AuthService';
import {Router} from '@angular/router';

@Component({
  selector: 'app-header',
  standalone: false,
  templateUrl: './header.html',
  styleUrl: './header.scss',
})
export class Header {
    constructor(public authService: AuthService, private router: Router) {}

  openLogin(): void {
      this.router.navigate(['home/auth/login']);
  }
  goToProfile(): void {
      this.router.navigate(['/profile']);
  }
  logout(): void {
      this.authService.logout();
      this.router.navigate(['/']);
  }

  goToCart() {
    this.router.navigate(['/cart']).catch(() => {});
  }
}
