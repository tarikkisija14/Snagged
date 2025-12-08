import { User } from './user';
import { CartItem } from './cart-item';

export interface Cart {
  id: number;
  userId: number;
  user?: User;
  createdAt: Date | string;
  updatedAt: Date | string;
  isSavedForLater: boolean;
  cartItems: CartItem[];
}
