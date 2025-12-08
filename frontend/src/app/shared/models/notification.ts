import { User } from './user';

export interface Notification {
  id: number;
  userId: number;
  user?: User;
  message: string;
  notificationType: string;
  isRead: boolean;
  createdAt: Date | string;
}
