import { Category } from './category';
import { ItemModel } from './item.model';

export interface Subcategory {
  id: number;
  categoryId: number;
  name?: string;
  category?: Category;
  items: ItemModel[];
}
