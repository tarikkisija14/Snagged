import { User } from './user';
import { Category } from './category';
import { Subcategory } from './subcategory';
import { OrderItem } from './order-item';
import { Favorite } from './favorite';
import { Report } from './report';
import { Conversation } from './conversation';
import { CartItem } from './cart-item';
import { ItemImageModel } from './item-image.model';
export interface ItemModel {
  id: number;
  userId?: number;
  title: string;
  description: string;
  price: number;
  condition: string;
  isSold: boolean;
  createdAt: Date | string;
  likesCount: number;

  images: ItemImageModel[];

  categoryId: number;
  subcategoryId?: number;

}
