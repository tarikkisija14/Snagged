import { Item } from './item';

export interface ItemImage {
  id: number;
  itemId: number;
  item?: Item;
  imageUrl: string;
  isMain: boolean;
}
