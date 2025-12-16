import { Injectable } from '@angular/core';
import {environment} from '../../../environments/development';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {Cart} from '../models/cart';

@Injectable({
  providedIn: 'root',
})
export class CartService {
  private apiUrl = `${environment.apiUrl}/cart`;

  constructor(private http: HttpClient) {}

  getAllCarts(): Observable<Cart[]> {
    return this.http.get<Cart[]>(this.apiUrl);
  }


  getCartByUser(userId: number): Observable<Cart> {
    return this.http.get<Cart>(`${this.apiUrl}/user/${userId}`);
  }

  addCartItem(userId: number, itemId: number, quantity: number = 1): Observable<number> {
    const body = { userId, itemId, quantity };
    console.log('Sending addCartItem payload:', body);
    return this.http.post<number>(`${this.apiUrl}/item`, body);
  }

  updateCartItem(cartItemId: number, quantity: number): Observable<void> {
    const body = { cartItemId, quantity };
    return this.http.put<void>(`${this.apiUrl}/item/${cartItemId}`, body);

  }

  deleteCartItem(cartItemId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/item/${cartItemId}`);
  }

  clearCart(userId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/user/${userId}/clear`);
  }



  checkout(userId: number): Observable<number> {
    const body = { userId };
    return this.http.post<number>(`${this.apiUrl}/checkout`, body);
  }

}

