import { Role } from './role';
import { ProfileModel } from './profile.model';
import { Cart } from './cart';
import { Address } from './address';
import { ItemModel } from './item.model';
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
  profile?: ProfileModel;
  cart?: Cart;
  addresses: Address[];
  items: ItemModel[];
  orders: Order[];
  favorites: Favorite[];
  reviewsGiven: Review[];
  reviewsReceived: Review[];
  reports: Report[];
  conversations: Conversation[];
  messages: Message[];
  notifications: Notification[];
}
