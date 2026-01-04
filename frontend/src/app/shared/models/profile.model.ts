import { User } from './user';
export interface ProfileModel {
  username: string;
  phoneNumber?: string;
  profileImageUrl?: string;
  bio: string;
  averageRating: number;
  reviewCount: number;
}
