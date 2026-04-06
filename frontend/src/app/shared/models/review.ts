export interface Review {
  id: number;
  reviewerId: number;
  reviewerUsername: string;
  reviewerProfileImageUrl?: string | null;
  reviewedUserId: number;
  rating: number;
  comment: string;
  createdAt: string;
  updatedAt: string;
}

export interface PagedReviews {
  total: number;
  items: Review[];
}

export type ReviewSortOrder = 'Newest' | 'HighestRating' | 'LowestRating';
