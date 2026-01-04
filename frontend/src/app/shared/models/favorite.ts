import { User } from './user';
import { ItemModel } from './item.model';

export interface Favorite {
  favoriteId: number;
  userId: number;
  user?: User;
  itemId: number;
  item?: ItemModel;
  addedAt: Date | string;
}
