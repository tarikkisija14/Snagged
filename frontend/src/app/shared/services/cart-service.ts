import { Injectable } from '@angular/core';
import { environment } from '../../../environments/development';
import { HttpClient } from '@angular/common/http';
import { EMPTY, Observable } from 'rxjs';
import { Cart } from '../models/cart';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth-service/AuthService';
import { Cart as CartModel } from '../../shared/models/cart';

@Injectable({
  providedIn: 'root',
})
export class CartService {
  private apiUrl = `${environment.apiUrl}/cart`;

  constructor(private http: HttpClient,
                private router:Router,
              private AuthService:AuthService) {}

  private cart: CartModel | null = null;

  getAllCarts(): Observable<Cart[]> {
    return this.http.get<Cart[]>(this.apiUrl);
  }

  setCart(cart: CartModel) {
    this.cart = cart;
  }

  getCart(): CartModel | null {
    return this.cart;
  }

  getCartByUser(): Observable<Cart> {
    const userId = this.AuthService.getUserId();
    if (!userId) {
      console.warn('User not logged in.');
      return EMPTY;
    }
    return this.http.get<Cart>(`${this.apiUrl}/user/${userId}`);
  }

  addCartItem(itemId: number, quantity: number = 1): Observable<number> {
    const userId = this.AuthService.getUserId();

    if (!userId) {
      console.warn('User not logged in.');
      return EMPTY;
    }

    const body = { userId, itemId, quantity };
    return this.http.post<number>(`${this.apiUrl}/item`, body);
  }

  updateCartItem(cartItemId: number, quantity: number): Observable<void> {
    const body = { cartItemId, quantity };
    return this.http.put<void>(`${this.apiUrl}/item/${cartItemId}`, body);

  }

  deleteCartItem(cartItemId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/item/${cartItemId}`);
  }

  clearCart(): Observable<void> {
    const userId = this.AuthService.getUserId();
    if (!userId) {
      console.warn('User not logged in.');
      return EMPTY;
    }
    return this.http.delete<void>(`${this.apiUrl}/user/${userId}/clear`);
  }

  createStripePaymentIntent(orderId: number): Observable<{ clientSecret: string }> {
    return this.http.post<{ clientSecret: string }>(
      `${environment.apiUrl}/payment/stripe/create-intent/${orderId}`,
      {}
    );
  }
  checkout(): Observable<number> {
    const userId = this.AuthService.getUserId();
    if (!userId) {
      console.warn('User not logged in.');
      return EMPTY;

    }
    return this.http.post<number>(`${this.apiUrl}/checkout`, { userId });
  }
  createPayment(data: { orderId: number; paymentMethod: string; amount: number; stripePaymentIntentId?: string }): Observable<number> {
    return this.http.post<number>(`${environment.apiUrl}/payment`, data);
  }

  getCartValue(): CartModel | null {
    return this.cart;
  }



}

