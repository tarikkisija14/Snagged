import { User } from './user';
import { ItemModel } from './item.model';

export interface Report {
  id: number;
  reporterId: number;
  reporter?: User;
  reportedItemId?: number;
  reportedItem?: ItemModel;
  reportedUserId?: number;
  reportedUser?: User;
  reason: string;
  createdAt: Date | string;
  status: string;
}
