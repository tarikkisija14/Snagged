import { Component } from '@angular/core';
import{Router} from '@angular/router';

@Component({
  selector: 'app-header',
  standalone: false,
  templateUrl: './header.html',
  styleUrl: './header.scss',
})
export class Header {

  constructor(private router: Router) { }
  goToCart() {
    this.router.navigate(['/cart']).catch(() => {});
  }
}
