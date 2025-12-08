import { Order } from './order';
import { Item } from './item';

export interface OrderItem {
  id: number;
  orderId: number;
  order?: Order;
  itemId: number;
  item?: Item;
  quantity: number;
  price: number;
}
