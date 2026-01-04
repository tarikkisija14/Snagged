import { Subcategory } from './subcategory';
import { ItemModel } from './item.model';

export interface Category {
  id: number;
  name: string;
  subcategories: Subcategory[];
  items: ItemModel[];
}
