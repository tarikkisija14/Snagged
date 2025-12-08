import { User } from './user';
import { Item } from './item';
import { Message } from './message';

export interface Conversation {
  id: number;
  userId: number;
  user?: User;
  itemId?: number;
  item?: Item;
  startedAt: Date | string;
  status: string;
  messages: Message[];
}
