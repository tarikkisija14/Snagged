import { ItemModel } from './item.model';

export interface ItemImageModel {
  id: number;
  itemId: number;
  item?: ItemModel;
  imageUrl: string;
  isMain: boolean;
}
