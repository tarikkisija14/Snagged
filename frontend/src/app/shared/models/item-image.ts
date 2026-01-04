import { ItemModel } from './item.model';

export interface ItemImage {
  id: number;
  itemId: number;
  item?: ItemModel;
  imageUrl: string;
  isMain: boolean;
}
