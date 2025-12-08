import { Role } from './role';
import { Profile } from './profile';
import { Cart } from './cart';
import { Address } from './address';
import { Item } from './item';
import { Order } from './order';
import { Favorite } from './favorite';
import { Review } from './review';
import { Report } from './report';
import { Conversation } from './conversation';
import { Message } from './message';
import { Notification } from './notification';

export interface User {
  id: number;
  email?: string;
  password?: string;
  createdAt: Date | string;
  roleId?: number;
  role?: Role;
  profile?: Profile;
  cart?: Cart;
  addresses: Address[];
  items: Item[];
  orders: Order[];
  favorites: Favorite[];
  reviewsGiven: Review[];
  reviewsReceived: Review[];
  reports: Report[];
  conversations: Conversation[];
  messages: Message[];
  notifications: Notification[];
}
