import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';

@Component({
  selector: 'app-payment-success',
  standalone: false,
  templateUrl: './payment-success.html',
  styleUrl: './payment-success.scss',
})
export class PaymentSuccess implements OnInit {
  countdown = 5;

  constructor(private router: Router) {}

  ngOnInit(): void {
    const interval = setInterval(() => {
      this.countdown--;
      if (this.countdown === 0) {
        clearInterval(interval);
        this.router.navigate(['/']); // redirect na homepage
      }
    }, 1000);
  }
}
