import { User } from './user';
import { Item } from './item';

export interface Report {
  id: number;
  reporterId: number;
  reporter?: User;
  reportedItemId?: number;
  reportedItem?: Item;
  reportedUserId?: number;
  reportedUser?: User;
  reason: string;
  createdAt: Date | string;
  status: string;
}
