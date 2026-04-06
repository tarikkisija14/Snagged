import { ItemImageModel } from './item-image.model';

export interface ItemModel {
  id: number;
  userId?: number;
  title: string;
  description: string;
  price: number;
  condition: string;
  isSold: boolean;
  createdAt: Date | string;
  likesCount: number;
  images: ItemImageModel[];
  categoryId: number;
  subcategoryId?: number;
  categoryName?: string;
  subcategoryName?: string;
  sellerUsername?: string;
}
