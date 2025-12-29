import {ChangeDetectorRef, Component, OnDestroy, OnInit} from '@angular/core';
import { Cart as CartModel } from '../../shared/models/cart';
import { CartItem } from '../../shared/models/cart-item';
import { CartService } from '../../shared/services/cart-service';
import{AuthService} from '../../core/services/auth-service/AuthService';
import {Router} from '@angular/router';
import {Subscription,BehaviorSubject,Observable,tap} from 'rxjs';



@Component({
  selector: 'app-cart',
  templateUrl: './cart.html',
  styleUrls: ['./cart.scss'],
  standalone:false
})
export class Cart implements OnInit,OnDestroy {
  cart: CartModel | null = null;
  userId:number|null = null;
  private authSub: Subscription | null = null;

  isLoading = false;

  constructor(private cartService: CartService,
              private cdr: ChangeDetectorRef,
              private authService:AuthService,
              private router:Router) {

  }

  ngOnInit(): void {
    this.authSub = this.authService.currentUser$.subscribe(userId => {
      if (!userId) {

        this.router.navigate(['/home/auth/login']);
        this.cart = null;
        return;
      }

      if (userId !== this.userId) {
        this.userId = userId;
        this.loadCart(this.userId);
      }
    });
  }

  ngOnDestroy(): void {
    if (this.authSub) this.authSub.unsubscribe();
  }

  loadCart(userId:number) {
    this.isLoading = true;


    this.cartService.getCartByUser().subscribe({
      next: (cartDto: any) => {


        this.cart = {
          id: cartDto.id,
          userId: cartDto.userId,
          createdAt: new Date(cartDto.createdAt),
          updatedAt: new Date(cartDto.updatedAt),
          isSavedForLater: false,
          cartItems: cartDto.items.map((i: any) => ({
            id: i.id,
            itemId: i.itemId,
            quantity: i.quantity,
            addedAt: new Date(i.addedAt),
            item: {
              title: i.itemName,
              price: i.price,
              imageUrl: i.imageUrl,
              imageUrls: i.imageUrls || []
            }
          }))
        };


        this.cartService.setCart(this.cart);
        this.isLoading = false;


        this.cdr.detectChanges();


        setTimeout(() => {
          this.cdr.detectChanges();
        }, 0);
      },
      error: (err) => {
        console.error(' Error loading cart:', err);
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }




  increaseQuantity(item: CartItem) {
    const newQuantity = item.quantity + 1;
    this.cartService.updateCartItem(item.id, newQuantity).subscribe(() => {
      item.quantity = newQuantity;
    });
  }

  decreaseQuantity(item: CartItem) {
    if (item.quantity <= 1) return;
    const newQuantity = item.quantity - 1;
    this.cartService.updateCartItem(item.id, newQuantity).subscribe(() => {
      item.quantity = newQuantity;
    });
  }

  removeItem(item: CartItem) {
    this.cartService.deleteCartItem(item.id).subscribe(() => {
      if (this.cart) {
        this.cart.cartItems = this.cart.cartItems.filter((ci) => ci.id !== item.id);
      }
    });
  }

  getSubtotal(): number {
    if (!this.cart) return 0;
    return this.cart.cartItems.reduce((sum, ci) => sum + ci.item!.price * ci.quantity, 0);
  }

  getShipping(): number {
    return 5.99;
  }

  getTax(): number {
    return +(this.getSubtotal() * 0.08).toFixed(2);
  }

  getTotal(): number {
    return +(this.getSubtotal() + this.getShipping() + this.getTax()).toFixed(2);
  }

  getItemImage(item: any): string {
    const baseUrl = 'https://localhost:7163';


    if (!item) {
      return `${baseUrl}/images/items/placeholder.png`;
    }





    let imageUrlValue = item.imageUrl;
    if (Array.isArray(imageUrlValue) && imageUrlValue.length === 0) {
      imageUrlValue = null;
    }


    if (item.imageUrls && Array.isArray(item.imageUrls) && item.imageUrls.length > 0) {
      const firstImage = item.imageUrls[0];
      if (firstImage) {

        if (firstImage.startsWith('http')) {
          return firstImage;
        }

        return `${baseUrl}${firstImage}`;
      }
    }


    if (imageUrlValue && typeof imageUrlValue === 'string' && imageUrlValue.trim() !== '') {
      if (imageUrlValue.startsWith('http')) {
        return imageUrlValue;
      }
      return `${baseUrl}${imageUrlValue}`;
    }


    return `${baseUrl}/images/items/placeholder.png`;
  }

  proceedToCheckout() {
    console.log('--- Proceed to Checkout clicked ---');

    if (!this.cart || this.cart.cartItems.length === 0) {
      console.warn('Cart je prazan ili undefined!');
      return;
    }

    console.log('Cart items:', this.cart.cartItems);

    // Dohvati userId iz observable
    this.authService.currentUser$.subscribe(userId => {
      if (!userId) {
        console.warn('User nije ulogovan!');
        return;
      }

      console.log('Current userId:', userId);

      this.cartService.checkout().subscribe({
        next: (orderId) => {
          console.log('Backend returned orderId:', orderId);
          this.router.navigate(['/payment', orderId]);

        },
        error: (err) => {
          console.error('Checkout failed, backend error:', err);
        }
      });
    });
  }







}
