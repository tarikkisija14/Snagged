import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { Cart as CartModel } from '../../shared/models/cart';
import { CartItem } from '../../shared/models/cart-item';
import { ItemModel } from '../../shared/models/item.model';
import { CartService, CartApiDto, CartItemApiDto } from '../../shared/services/cart-service';
import { AuthService } from '../../core/services/auth-service/AuthService';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { environment } from '../../../environments/environment';
import { SHIPPING_COST, TAX_RATE } from '../../shared/constants/order-total.constants';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.html',
  styleUrls: ['./cart.scss'],
  standalone: false
})
export class Cart implements OnInit, OnDestroy {
  cart: CartModel | null = null;
  isLoading = false;

  readonly shippingCost = SHIPPING_COST;
  readonly taxRate      = TAX_RATE;

  private readonly baseImageUrl = environment.apiUrl.replace('/api', '');
  private authSub: Subscription | null = null;

  constructor(
    private cartService: CartService,
    private cdr: ChangeDetectorRef,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.authSub = this.authService.currentUser$.subscribe(userId => {
      if (!userId) {
        this.router.navigate(['/home/auth/login']);
        this.cart = null;
        return;
      }
      this.loadCart();
    });
  }

  ngOnDestroy(): void {
    this.authSub?.unsubscribe();
  }

  loadCart(): void {
    this.isLoading = true;
    this.cartService.getCartByUser().subscribe({
      next: (cartDto: CartApiDto) => {
        this.cart = this.mapCartDto(cartDto);
        this.cartService.setCart(this.cart);
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }


  private mapCartDto(dto: CartApiDto): CartModel {
    return {
      id:              dto.id,
      userId:          dto.userId,
      createdAt:       new Date(dto.createdAt),
      updatedAt:       new Date(dto.updatedAt),
      isSavedForLater: false,
      cartItems: dto.items.map((i: CartItemApiDto): CartItem => ({
        id:       i.id,
        cartId:   dto.id,
        itemId:   i.itemId,
        quantity: i.quantity,
        addedAt:  new Date(i.addedAt),
        price:    i.price,
        imageUrl: i.imageUrl ?? '',
        // Populate item so the template can read cartItem.item.title / .price
        item: {
          id:          i.itemId,
          title:       i.itemName,
          price:       i.price,
          description: '',
          condition:   '',
          isSold:      false,
          createdAt:   new Date(),
          likesCount:  0,
          images:      [],
          categoryId:  0,
        } as ItemModel
      }))
    };
  }

  increaseQuantity(item: CartItem): void {
    const newQty = item.quantity + 1;
    this.cartService.updateCartItem(item.id, newQty).subscribe(() => {
      item.quantity = newQty;
      this.cdr.detectChanges();
    });
  }

  decreaseQuantity(item: CartItem): void {
    if (item.quantity <= 1) return;
    const newQty = item.quantity - 1;
    this.cartService.updateCartItem(item.id, newQty).subscribe(() => {
      item.quantity = newQty;
      this.cdr.detectChanges();
    });
  }

  removeItem(item: CartItem): void {
    this.cartService.deleteCartItem(item.id).subscribe(() => {
      if (this.cart)
        this.cart.cartItems = this.cart.cartItems.filter(ci => ci.id !== item.id);
      this.cdr.detectChanges();
    });
  }

  getSubtotal(): number {
    if (!this.cart) return 0;
    return this.cart.cartItems.reduce(
      (sum, ci) => sum + ci.price * ci.quantity, 0
    );
  }

  getShipping(): number { return this.shippingCost; }

  getTax(): number { return +(this.getSubtotal() * this.taxRate).toFixed(2); }

  getTotal(): number {
    return +(this.getSubtotal() + this.getShipping() + this.getTax()).toFixed(2);
  }


  getItemImage(item: ItemModel | undefined): string {
    if (!item) return `${this.baseImageUrl}/images/items/placeholder.png`;

    // Find the CartItem that owns this ItemModel to read its top-level imageUrl
    const cartItem = this.cart?.cartItems.find(ci => ci.itemId === item.id);
    const imageUrl = cartItem?.imageUrl?.trim()
      || item.images?.[0]?.imageUrl?.trim();

    if (imageUrl) {
      return imageUrl.startsWith('http')
        ? imageUrl
        : `${this.baseImageUrl}${imageUrl}`;
    }
    return `${this.baseImageUrl}/images/items/placeholder.png`;
  }

  proceedToCheckout(): void {
    if (!this.cart?.cartItems.length) return;

    this.cartService.checkout().subscribe({
      next: ({ orderId }) => this.router.navigate(['/payment', orderId]),
      error: (err) => console.error('Checkout failed:', err)
    });
  }
}
