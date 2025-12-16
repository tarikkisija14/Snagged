import {ChangeDetectorRef, Component, OnInit} from '@angular/core';
import { Cart as CartModel } from '../../shared/models/cart';
import { CartItem } from '../../shared/models/cart-item';
import { CartService } from '../../shared/services/cart-service';


@Component({
  selector: 'app-cart',
  templateUrl: './cart.html',
  styleUrls: ['./cart.scss'],
  standalone:false
})
export class Cart implements OnInit {
  cart: CartModel | null = null;
  userId: number = 1;
  isLoading = false;

  constructor(private cartService: CartService,
              private cdr: ChangeDetectorRef) {

  }

  ngOnInit(): void {
    this.loadCart();
  }

  loadCart() {
    this.isLoading = true;


    this.cartService.getCartByUser(this.userId).subscribe({
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





}
