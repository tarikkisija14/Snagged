import { Component, Input, Output, EventEmitter } from '@angular/core';
import { ItemModel } from '../../models/item.model';

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

  @Output() addToCartClick = new EventEmitter<ItemModel>();
  @Output() viewDetailsClick = new EventEmitter<ItemModel>();

  addedToCart = false;


  getItemImage(): string {
    if (this.item?.images && this.item.images.length > 0) {
      const mainImage = this.item.images.find(img => img.isMain);

      if (mainImage?.imageUrl) {
        return this.getFullImageUrl(mainImage.imageUrl);
      }

      if (this.item.images[0]?.imageUrl) {
        return this.getFullImageUrl(this.item.images[0].imageUrl);
      }
    }


    return 'https://placehold.co/400x600/e0e0e0/666?text=No+Image';
  }


  private getFullImageUrl(imageUrl: string): string {
    // Check if it's an external URL
    if (imageUrl.startsWith('http://') || imageUrl.startsWith('https://')) {
      return imageUrl;
    }


    return `https://localhost:7163/images/items/${imageUrl}`;
  }


  onAddToCart(): void {
    if (!this.item) return;

    this.addedToCart = true;
    this.addToCartClick.emit(this.item);


    setTimeout(() => {
      this.addedToCart = false;
    }, 2000);
  }


  onViewDetails(): void {
    if (!this.item) return;

    this.viewDetailsClick.emit(this.item);
  }

  onImageError(event: Event): void {
    const imgElement = event.target as HTMLImageElement;
    imgElement.src = 'assets/images/placeholder.jpg';
  }
}
