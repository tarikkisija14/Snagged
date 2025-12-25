import { Component } from '@angular/core';
import {Router} from '@angular/router';


@Component({
  selector: 'app-auth-wrapper',
  standalone: false,
  templateUrl: './auth-wrapper.html',
  styleUrl: './auth-wrapper.scss',
})
export class AuthWrapper {
  constructor(public router: Router) { }

  isLogin(): boolean{
    return this.router.url.includes('login');
  }
  isRegister():boolean{
    return this.router.url.includes('register');
  }
  goHome() {
    this.router.navigate(['/']);
  }
  ngOnInit() {
    document.body.style.overflow = 'hidden';
  }

  ngOnDestroy() {
    document.body.style.overflow = '';
  }

}
