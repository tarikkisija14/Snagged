export interface ProfileModel {
  userId: number;
  username: string;
  phoneNumber?: string;
  profileImageUrl?: string;
  bio: string;
  averageRating: number;
  reviewCount: number;
}
