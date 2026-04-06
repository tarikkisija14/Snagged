import {
  Component,
  Input,
  Output,
  EventEmitter,
} from '@angular/core';
import { Router } from '@angular/router';
import { ItemModel } from '../../models/item.model';
import { AuthService } from '../../../core/services/auth-service/AuthService';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-product-card',
  standalone: false,
  templateUrl: './product-card.component.html',
  styleUrls: ['./product-card.component.scss'],
})
export class ProductCardComponent {
  @Input() item!: ItemModel;
  @Input() categoryName: string | null = null;
  @Input() showActions: boolean = true;
  @Input() showBadge: boolean = true;
  @Input() showDetails: boolean = true;

  @Output() addToCartClick   = new EventEmitter<ItemModel>();
  @Output() viewDetailsClick = new EventEmitter<ItemModel>();

  addedToCart = false;

  private readonly baseUrl = environment.apiUrl.replace('/api', '');

  constructor(
    private router: Router,
    private authService: AuthService,
  ) {}

  getItemImage(): string {
    if (this.item?.images?.length > 0) {
      const mainImage = this.item.images.find(img => img.isMain) ?? this.item.images[0];
      if (mainImage?.imageUrl) {
        return this.resolveImageUrl(mainImage.imageUrl);
      }
    }
    return `${this.baseUrl}/images/items/placeholder.png`;
  }

  private resolveImageUrl(imageUrl: string): string {
    if (imageUrl.startsWith('http://') || imageUrl.startsWith('https://')) {
      return imageUrl;
    }
    return `${this.baseUrl}${imageUrl}`;
  }

  onAddToCart(): void {
    if (!this.item || this.item.isSold || this.addedToCart) return;
    if (!this.authService.isLoggedIn()) {
      this.router.navigate(['/home/auth/login']);
      return;
    }
    this.addedToCart = true;
    this.addToCartClick.emit(this.item);
    setTimeout(() => {
      this.addedToCart = false;
    }, 2000);
  }

  onViewDetails(): void {
    if (!this.item) return;
    this.viewDetailsClick.emit(this.item);
    this.router.navigate(['/items', this.item.id]);
  }

  onImageError(event: Event): void {
    (event.target as HTMLImageElement).src =
      `${this.baseUrl}/images/items/placeholder.png`;
  }
}
