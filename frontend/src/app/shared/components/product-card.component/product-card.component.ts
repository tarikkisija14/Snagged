import { Component, Input,Output, EventEmitter } from '@angular/core';
import {ItemModel} from '../../models/item.model';

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

  @Output() addToCartClick = new EventEmitter<any>();
  @Output() viewDetailsClick = new EventEmitter<any>();
  addedToCart = false;

  getItemImage(): string {
    if (this.item?.images && this.item.images.length > 0) {
      const mainImage = this.item.images.find(img => img.isMain);

      if (mainImage?.imageUrl) {
        return mainImage.imageUrl;
      }

      // If no main image, use the first image
      if (this.item.images[0]?.imageUrl) {
        return this.item.images[0].imageUrl;
      }
    }

    return 'assets/images/placeholder.jpg';
  }

  onAddToCart() {
    this.addedToCart = true;
    this.addToCartClick.emit(this.item);

    setTimeout(() => {
      this.addedToCart = false;
    }, 2000);
  }

  onViewDetails() {
    this.viewDetailsClick.emit(this.item);
  }
}
