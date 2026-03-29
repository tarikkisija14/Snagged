import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { loadStripe, Stripe, StripeCardElement, StripeElements } from '@stripe/stripe-js';
import { environment } from '../../../environments/environment';
import { CartService } from '../../shared/services/cart-service';
import { SHIPPING_COST, TAX_RATE } from '../../shared/constants/order-total.constants';

@Component({
  selector: 'app-payment',
  templateUrl: './payment.html',
  styleUrls: ['./payment.scss'],
  standalone: false
})
export class Payment implements OnInit {
  orderId!: number;
  stripe!: Stripe | null;
  elements!: StripeElements;
  cardElement!: StripeCardElement;
  message      = '';
  errorMessage = '';
  loading      = false;


  totalAmount: number | null = null;

  constructor(
    private route: ActivatedRoute,
    private cartService: CartService,
    private router: Router,
    private cd: ChangeDetectorRef
  ) {}

  async ngOnInit(): Promise<void> {
    this.orderId = Number(this.route.snapshot.paramMap.get('orderId'));
    this.stripe  = await loadStripe(environment.stripePublicKey);

    if (this.stripe) {
      this.elements    = this.stripe.elements();
      this.cardElement = this.elements.create('card');
      this.cardElement.mount('#card-element');
    }


    this.totalAmount = this.calculateTotal();
  }

  async pay(): Promise<void> {
    if (!this.stripe || !this.cardElement) return;

    this.loading      = true;
    this.errorMessage = '';
    this.message      = '';

    this.cartService.createStripePaymentIntent(this.orderId).subscribe({
      next: async ({ clientSecret }) => {
        const result = await this.stripe!.confirmCardPayment(clientSecret, {
          payment_method: { card: this.cardElement }
        });

        this.loading = false;

        if (result.error) {
          this.errorMessage = result.error.message ?? 'Payment failed.';
        } else if (result.paymentIntent?.status === 'succeeded') {
          this.message = 'Payment successful! Redirecting...';
          this.router.navigate(['/payment-success'], {
            queryParams: { orderId: this.orderId }
          });
        }

        this.cd.detectChanges();
      },
      error: () => {
        this.loading      = false;
        this.errorMessage = 'Payment failed due to a server error.';
        this.cd.detectChanges();
      }
    });
  }


  private calculateTotal(): number | null {
    const cart = this.cartService.getCart();
    if (!cart) return null;

    const subtotal = cart.cartItems.reduce(
      (sum, ci) => sum + ci.item!.price * ci.quantity, 0
    );
    return +(subtotal + SHIPPING_COST + subtotal * TAX_RATE).toFixed(2);
  }
}
