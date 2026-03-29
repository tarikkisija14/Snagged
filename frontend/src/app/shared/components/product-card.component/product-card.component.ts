import { Component, Input, Output, EventEmitter } from '@angular/core';
import { ItemModel } from '../../models/item.model';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-product-card',
  standalone: false,
  templateUrl: './product-card.component.html',
  styleUrl: './product-card.component.scss',
})
export class ProductCardComponent {
  @Input() item!: ItemModel;
  @Input() showActions: boolean = true;
  @Input() showBadge: boolean = true;
  @Input() showRating: boolean = true;
  @Input() showDetails: boolean = true;

  @Output() addToCartClick    = new EventEmitter<ItemModel>();
  @Output() viewDetailsClick  = new EventEmitter<ItemModel>();

  addedToCart = false;

  private readonly baseUrl = environment.apiUrl.replace('/api', '');

  getItemImage(): string {
    if (this.item?.images?.length > 0) {
      const mainImage = this.item.images.find(img => img.isMain) ?? this.item.images[0];
      if (mainImage?.imageUrl) {
        return this.resolveImageUrl(mainImage.imageUrl);
      }
    }
    return 'https://placehold.co/400x600/e0e0e0/666?text=No+Image';
  }

  private resolveImageUrl(imageUrl: string): string {
    if (imageUrl.startsWith('http://') || imageUrl.startsWith('https://')) {
      return imageUrl;
    }
    return `${this.baseUrl}${imageUrl}`;
  }

  onAddToCart(): void {
    if (!this.item) return;
    this.addedToCart = true;
    this.addToCartClick.emit(this.item);
    setTimeout(() => { this.addedToCart = false; }, 2000);
  }

  onViewDetails(): void {
    if (!this.item) return;
    this.viewDetailsClick.emit(this.item);
  }

  onImageError(event: Event): void {
    (event.target as HTMLImageElement).src = 'https://placehold.co/400x600/e0e0e0/666?text=No+Image';
  }
}
