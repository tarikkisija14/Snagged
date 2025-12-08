import { User } from './user';

export interface Review {
  id: number;
  reviewerId: number;
  reviewer?: User;
  reviewedUserId: number;
  reviewedUser?: User;
  rating: number;
  comment: string;
  createdAt: Date | string;
}
