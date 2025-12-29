import { Component, OnInit,ChangeDetectorRef } from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import { loadStripe, Stripe, StripeCardElement, StripeElements } from '@stripe/stripe-js';
import { environment } from '../../../environments/environment';
import { CartService } from '../../shared/services/cart-service';

@Component({
  selector: 'app-payment',
  templateUrl: './payment.html',
  styleUrls: ['./payment.scss'],
  standalone:false
})
export class Payment implements OnInit {
  orderId!: number;
  stripe!: Stripe | null;
  elements!: StripeElements;
  cardElement!: StripeCardElement;
  message = '';
  loading = false;
  totalAmount: number = 0;
  errorMessage = '';

  constructor(
    private route: ActivatedRoute,
    private cartService: CartService,
    private router:Router,
    private cd: ChangeDetectorRef

  ) {}

  async ngOnInit() {
    console.log('[1] Payment component INIT');

    this.orderId = Number(this.route.snapshot.paramMap.get('orderId'));
    console.log('[2] OrderId from route:', this.orderId);

    console.log('[3] Loading Stripe with key:', environment.stripePublicKey);
    this.stripe = await loadStripe(environment.stripePublicKey);

    console.log('[4] Stripe loaded:', this.stripe);

    if (this.stripe) {
      this.elements = this.stripe.elements();
      console.log('[5] Stripe elements created:', this.elements);

      this.cardElement = this.elements.create('card');
      console.log('[6] Card element created:', this.cardElement);

      this.cardElement.mount('#card-element');
      console.log('[7] Card element mounted');
    }

    const cart = this.cartService.getCartValue();
    console.log('[8] Cart value:', cart);

    if (cart) {
      const subtotal = cart.cartItems.reduce(
        (sum, ci) => sum + ci.item!.price * ci.quantity,
        0
      );

      const shipping = 5.99;
      const tax = subtotal * 0.08;

      this.totalAmount = +(subtotal + shipping + tax).toFixed(2);

      console.log('[9] Calculated totals:', {
        subtotal,
        shipping,
        tax,
        total: this.totalAmount
      });
    }
  }


  async pay() {


    if (!this.stripe || !this.cardElement) {

      return;
    }

    this.loading = true;
    console.log('[12] Creating PaymentIntent for orderId:', this.orderId);

    this.cartService.createStripePaymentIntent(this.orderId)
      .subscribe({
        next: async (res: { clientSecret: string }) => {




          const result = await this.stripe!.confirmCardPayment(
            res.clientSecret,
            {
              payment_method: {
                card: this.cardElement
              }
            }
          );



          this.loading = false;

          if (result.error) {
            this.message = '';
            this.errorMessage = result.error.message ?? 'Payment failed';
            this.cd.detectChanges();
          }
          else if (result.paymentIntent) {
            console.log('[17] PaymentIntent status:', result.paymentIntent.status);

            if (result.paymentIntent.status === 'succeeded') {

              this.message = 'Payment successful! Redirecting to confirmation page...';


              this.router.navigate(['/payment-success'], { queryParams: { orderId: this.orderId } });
            }
          }
        },
        error: err => {
          this.loading = false;
          this.message = '';
          this.errorMessage = 'Payment failed due to server error';
          this.cd.detectChanges();
        }
      });
  }

}
