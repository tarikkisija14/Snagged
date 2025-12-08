import { Cart } from './cart';
import { Item } from './item';

export interface CartItem {
  id: number;
  cartId: number;
  cart?: Cart;
  itemId: number;
  item?: Item;
  quantity: number;
  addedAt: Date | string;
}
