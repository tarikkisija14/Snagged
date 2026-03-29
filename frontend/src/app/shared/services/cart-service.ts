import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { EMPTY, Observable } from 'rxjs';
import { Cart } from '../models/cart';
import { AuthService } from '../../core/services/auth-service/AuthService';

/** Raw shape returned by GET /api/cart — mapped to Cart model in the component */
export interface CartItemApiDto {
  id: number;
  itemId: number;
  itemName: string;
  quantity: number;
  addedAt: string;
  imageUrl: string | null;
  price: number;
}

export interface CartApiDto {
  id: number;
  userId: number;
  createdAt: string;
  updatedAt: string;
  items: CartItemApiDto[];
}

@Injectable({
  providedIn: 'root',
})
export class CartService {
  private readonly apiUrl = `${environment.apiUrl}/cart`;

  // In-memory cache of the last loaded cart — used by the payment page to show the total
  private cart: Cart | null = null;

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) {}

  setCart(cart: Cart): void {
    this.cart = cart;
  }

  getCart(): Cart | null {
    return this.cart;
  }

  /**
   * Returns the raw DTO from the API.
   * The component is responsible for mapping it to the Cart model.
   * SECURITY FIX: userId is no longer sent in the URL — the backend resolves it from the JWT.
   * ROUTE FIX: was /cart/user/:userId — now /cart
   */
  getCartByUser(): Observable<CartApiDto> {
    if (!this.authService.isLoggedIn()) {
      console.warn('CartService: user is not logged in.');
      return EMPTY;
    }
    return this.http.get<CartApiDto>(this.apiUrl);
  }

  /**
   * SECURITY FIX: userId removed from the request body.
   * The backend resolves it from the JWT token.
   */
  addCartItem(itemId: number, quantity: number = 1): Observable<number> {
    if (!this.authService.isLoggedIn()) {
      console.warn('CartService: user is not logged in.');
      return EMPTY;
    }
    return this.http.post<number>(`${this.apiUrl}/item`, { itemId, quantity });
  }

  updateCartItem(cartItemId: number, quantity: number): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/item/${cartItemId}`, { cartItemId, quantity });
  }

  deleteCartItem(cartItemId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/item/${cartItemId}`);
  }

  /**
   * SECURITY FIX: userId removed from URL.
   * ROUTE FIX: was /cart/user/:userId/clear — now /cart/clear
   */
  clearCart(): Observable<void> {
    if (!this.authService.isLoggedIn()) {
      console.warn('CartService: user is not logged in.');
      return EMPTY;
    }
    return this.http.delete<void>(`${this.apiUrl}/clear`);
  }

  /**
   * BUG FIX: was typed Observable<number> but API returns { orderId: number }
   * SECURITY FIX: userId removed from request body
   */
  checkout(): Observable<{ orderId: number }> {
    if (!this.authService.isLoggedIn()) {
      console.warn('CartService: user is not logged in.');
      return EMPTY;
    }
    return this.http.post<{ orderId: number }>(`${this.apiUrl}/checkout`, {});
  }

  createStripePaymentIntent(orderId: number): Observable<{ clientSecret: string }> {
    return this.http.post<{ clientSecret: string }>(
      `${environment.apiUrl}/payment/stripe/create-intent/${orderId}`,
      {}
    );
  }

  createPayment(data: {
    orderId: number;
    paymentMethod: string;
    amount: number;
    stripePaymentIntentId?: string;
  }): Observable<{ paymentId: number }> {
    return this.http.post<{ paymentId: number }>(`${environment.apiUrl}/payment`, data);
  }
}
