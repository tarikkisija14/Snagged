import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

import { ItemModel } from '../../shared/models/item.model';
import { ItemImageModel } from '../../shared/models/item-image.model';
import { ItemService } from '../../shared/services/item-service';
import { CartService } from '../../shared/services/cart-service';
import { AuthService } from '../../core/services/auth-service/AuthService';
import { ReviewService } from '../../shared/services/review-service';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-item-detail',
  templateUrl: './item-detail.html',
  styleUrls: ['./item-detail.scss'],
  standalone: false,
})
export class ItemDetailComponent implements OnInit, OnDestroy {
  item: ItemModel | null = null;
  isLoading   = true;
  notFound    = false;
  addedToCart = false;
  cartError   = '';

  selectedImage: ItemImageModel | null = null;
  lightboxOpen  = false;

  averageRating = 0;
  reviewCount   = 0;

  private readonly baseUrl = environment.apiUrl.replace('/api', '');
  private destroy$ = new Subject<void>();

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private itemService: ItemService,
    private cartService: CartService,
    private authService: AuthService,
    private reviewService: ReviewService,
    private cdr: ChangeDetectorRef,
  ) {}

  ngOnInit(): void {
    this.route.paramMap
      .pipe(takeUntil(this.destroy$))
      .subscribe(params => {
        const id = Number(params.get('id'));
        if (!id || isNaN(id)) {
          this.notFound  = true;
          this.isLoading = false;
          return;
        }
        this.loadItem(id);
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private loadItem(id: number): void {
    this.isLoading     = true;
    this.notFound      = false;
    this.item          = null;
    this.averageRating = 0;
    this.reviewCount   = 0;

    this.itemService.getItemById(id)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (item) => {
          this.item          = item;
          this.selectedImage = item.images?.find(img => img.isMain) ?? item.images?.[0] ?? null;
          this.isLoading     = false;
          this.cdr.markForCheck();

          if (item.userId && !this.isOwnItem) {
            this.loadSellerRatingSummary(item.userId);
          }
        },
        error: () => {
          this.notFound  = true;
          this.isLoading = false;
          this.cdr.markForCheck();
        },
      });
  }

  private loadSellerRatingSummary(sellerId: number): void {
    console.log('[ItemDetail] loadSellerRatingSummary - sellerId:', sellerId);
    this.reviewService
      .getPagedReviews(sellerId, 1, 1, 'Newest')
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (result) => {
          console.log('[ItemDetail] getPagedReviews result:', result);
          console.log('[ItemDetail] averageRating BEFORE:', this.averageRating, '| reviewCount BEFORE:', this.reviewCount);
          this.reviewCount = result.total;
          console.log('[ItemDetail] averageRating AFTER:', this.averageRating, '| reviewCount AFTER:', this.reviewCount);
          this.cdr.markForCheck();
        },
        error: () => {},
      });
  }

  selectImage(image: ItemImageModel): void {
    this.selectedImage = image;
  }

  openLightbox(): void {
    this.lightboxOpen = true;
  }

  closeLightbox(): void {
    this.lightboxOpen = false;
  }

  resolveUrl(imageUrl: string | undefined | null): string {
    if (!imageUrl) return `${this.baseUrl}/images/items/placeholder.png`;
    if (imageUrl.startsWith('http://') || imageUrl.startsWith('https://')) return imageUrl;
    return `${this.baseUrl}${imageUrl}`;
  }

  get mainImageUrl(): string {
    return this.resolveUrl(this.selectedImage?.imageUrl);
  }

  onImageError(event: Event): void {
    (event.target as HTMLImageElement).src = `${this.baseUrl}/images/items/placeholder.png`;
  }

  get isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }

  get isOwnItem(): boolean {
    const currentUserId = this.authService.getUserId();
    return (
      currentUserId !== null &&
      this.item?.userId !== undefined &&
      this.item.userId !== null &&
      currentUserId === this.item.userId
    );
  }

  addToCart(): void {
    if (!this.item || this.item.isSold) return;
    this.cartError = '';

    this.cartService.addCartItem(this.item.id, 1)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          this.addedToCart = true;
          this.cdr.markForCheck();
          setTimeout(() => {
            this.addedToCart = false;
            this.cdr.markForCheck();
          }, 2500);
        },
        error: () => {
          this.cartError = 'Could not add item to cart. Please try again.';
          this.cdr.markForCheck();
        },
      });
  }

  goToSeller(): void {
    if (!this.item?.userId) return;
    if (this.isOwnItem) {
      this.router.navigate(['/profile']);
    } else {
      const reviewsEl = document.querySelector('.seller-reviews');
      if (reviewsEl) {
        reviewsEl.scrollIntoView({ behavior: 'smooth' });
      }
    }
  }

  goBack(): void {
    this.router.navigate(['/shop']);
  }
}
