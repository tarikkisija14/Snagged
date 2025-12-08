import { Subcategory } from './subcategory';
import { Item } from './item';

export interface Category {
  id: number;
  name: string;
  subcategories: Subcategory[];
  items: Item[];
}
