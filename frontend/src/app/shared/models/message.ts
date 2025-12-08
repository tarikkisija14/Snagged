import { Conversation } from './conversation';
import { User } from './user';

export interface Message {
  id: number;
  conversationId: number;
  conversation?: Conversation;
  senderId: number;
  sender?: User;
  content: string;
  sentAt: Date | string;
  isRead: boolean;
}
