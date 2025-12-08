import { Category } from './category';
import { Item } from './item';

export interface Subcategory {
  id: number;
  categoryId: number;
  name?: string;
  category?: Category;
  items: Item[];
}
