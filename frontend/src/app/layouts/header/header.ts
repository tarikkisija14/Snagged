import { Component, OnInit } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import {AuthService} from '../../core/services/auth-service/AuthService';
import {Router} from '@angular/router';

@Component({
  selector: 'app-header',
  standalone: false,
  templateUrl: './header.html',
  styleUrl: './header.scss',
})
export class Header {
  isMobile = false;

  constructor(
    private breakpointObserver: BreakpointObserver,
    public authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.breakpointObserver
      .observe([Breakpoints.Handset, Breakpoints.Tablet])
      .subscribe(result => {
        this.isMobile = result.matches;
      });
  }

  openLogin(): void {
      this.router.navigate(['home/auth/login']);
  }

  onUserIconClick() {
    if (this.authService.isLoggedIn()) {
      this.router.navigate(['/profile']);
    } else {
      this.router.navigate(['/home/auth/login']);
    }
  }

  logout(): void {
      this.authService.logout();
      this.router.navigate(['/']);
  }

  goToCart() {
    this.router.navigate(['/cart']).catch(() => {});
  }
}
