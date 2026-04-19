import { ChangeDetectorRef, Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { loadStripe, Stripe, StripeCardElement, StripeElements } from '@stripe/stripe-js';
import { environment } from '../../../environments/environment';
import { CartService } from '../../shared/services/cart-service';
import { SHIPPING_COST, TAX_RATE } from '../../shared/constants/order-total.constants';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-payment',
  templateUrl: './payment.html',
  styleUrls: ['./payment.scss'],
  standalone: false,
})
export class Payment implements OnInit, AfterViewInit, OnDestroy {
  orderId!: number;
  stripe!: Stripe | null;
  elements!: StripeElements;
  cardElement!: StripeCardElement;
  message      = '';
  errorMessage = '';
  loading      = false;


  stripeReady  = false;

  totalAmount: number | null = null;

  constructor(
    private route: ActivatedRoute,
    private cartService: CartService,
    private router: Router,
    private cd: ChangeDetectorRef,
    private http: HttpClient,
  ) {}

  async ngOnInit(): Promise<void> {
    this.orderId     = Number(this.route.snapshot.paramMap.get('orderId'));
    this.totalAmount = this.calculateTotalFromCart();
    if (this.totalAmount === null) {
      this.fetchOrderTotal();
    }

    this.stripe = await loadStripe(environment.stripePublicKey);

    if (this.stripe) {
      this.elements    = this.stripe.elements();
      this.cardElement = this.elements.create('card');


      setTimeout(() => {
        this.stripeReady = true;
        this.cd.detectChanges();
        this.cardElement.mount('#card-element');
      });
    }
  }

  ngAfterViewInit(): void {

  }

  ngOnDestroy(): void {

    this.cardElement?.destroy();
  }

  async pay(): Promise<void> {
    if (!this.stripe || !this.cardElement) return;

    this.loading      = true;
    this.errorMessage = '';
    this.message      = '';

    this.cartService.createStripePaymentIntent(this.orderId).subscribe({
      next: async ({ clientSecret }) => {
        const result = await this.stripe!.confirmCardPayment(clientSecret, {
          payment_method: { card: this.cardElement },
        });

        this.loading = false;

        if (result.error) {
          this.errorMessage = result.error.message ?? 'Payment failed.';
        } else if (result.paymentIntent?.status === 'succeeded') {
          this.message = 'Payment successful! Redirecting...';
          this.router.navigate(['/payment-success'], {
            queryParams: { orderId: this.orderId },
          });
        }

        this.cd.detectChanges();
      },
      error: () => {
        this.loading      = false;
        this.errorMessage = 'Payment failed due to a server error. Please try again.';
        this.cd.detectChanges();
      },
    });
  }

  private calculateTotalFromCart(): number | null {
    const cart = this.cartService.getCart();
    if (!cart || !cart.cartItems.length) return null;

    const subtotal = cart.cartItems.reduce(
      (sum, ci) => sum + ci.price * ci.quantity, 0
    );
    return +(subtotal + SHIPPING_COST + subtotal * TAX_RATE).toFixed(2);
  }

  private fetchOrderTotal(): void {
    this.http
      .get<{ orderItems: { price: number; quantity: number }[] }>(
        `${environment.apiUrl}/orders/${this.orderId}`
      )
      .subscribe({
        next: (order) => {
          if (order?.orderItems?.length) {
            const subtotal = order.orderItems.reduce(
              (sum, oi) => sum + oi.price * oi.quantity, 0
            );
            this.totalAmount = +(subtotal + SHIPPING_COST + subtotal * TAX_RATE).toFixed(2);
            this.cd.detectChanges();
          }
        },
        error: () => {

        },
      });
  }
}
