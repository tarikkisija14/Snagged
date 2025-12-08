import { User } from './user';
import { Item } from './item';

export interface Favorite {
  favoriteId: number;
  userId: number;
  user?: User;
  itemId: number;
  item?: Item;
  addedAt: Date | string;
}
