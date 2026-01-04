import { User } from './user';
import { ItemModel } from './item.model';
import { Message } from './message';

export interface Conversation {
  id: number;
  userId: number;
  user?: User;
  itemId?: number;
  item?: ItemModel;
  startedAt: Date | string;
  status: string;
  messages: Message[];
}
