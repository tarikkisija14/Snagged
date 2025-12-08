import { User } from './user';

export interface Profile {
  id: number;
  userId: number;
  user?: User;
  username: string;
  phoneNumber: string;
  profileImageUrl?: string;
  bio: string;
  averageRating: number;
  reviewCount: number;
}
