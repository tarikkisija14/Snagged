import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-payment-success',
  standalone: false,
  templateUrl: './payment-success.html',
  styleUrl: './payment-success.scss',
})
export class PaymentSuccess implements OnInit, OnDestroy {
  countdown = 5;

  private countdownInterval?: ReturnType<typeof setInterval>;

  constructor(private router: Router) {}

  ngOnInit(): void {
    this.countdownInterval = setInterval(() => {
      this.countdown--;
      if (this.countdown === 0) {
        this.clearCountdown();
        this.router.navigate(['/']);
      }
    }, 1000);
  }

  ngOnDestroy(): void {
    this.clearCountdown();
  }

  private clearCountdown(): void {
    if (this.countdownInterval !== undefined) {
      clearInterval(this.countdownInterval);
      this.countdownInterval = undefined;
    }
  }
}
