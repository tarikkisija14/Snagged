import { User } from './user';
import { Category } from './category';
import { Subcategory } from './subcategory';
import { ItemImage } from './item-image';
import { OrderItem } from './order-item';
import { Favorite } from './favorite';
import { Report } from './report';
import { Conversation } from './conversation';
import { CartItem } from './cart-item';

export interface Item {
  id: number;
  userId: number;
  user?: User;
  categoryId: number;
  category?: Category;
  subcategoryId?: number;
  subcategory?: Subcategory;
  title: string;
  description: string;
  price: number;
  condition: string;
  isSold: boolean;
  createdAt: Date | string;
  images: ItemImage[];
  orderItems: OrderItem[];
  favorites: Favorite[];
  reports: Report[];
  conversations: Conversation[];
  cartItems: CartItem[];
}
