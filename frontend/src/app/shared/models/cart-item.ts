import { Cart } from './cart';
import { ItemModel } from './item.model';

export interface CartItem {
  id: number;
  cartId: number;
  cart?: Cart;
  itemId: number;
  item?: ItemModel;
  quantity: number;
  addedAt: Date | string;
  price: number;
  imageUrl: string;
}
