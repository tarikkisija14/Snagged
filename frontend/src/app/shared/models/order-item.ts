import { Order } from './order';
import { ItemModel } from './item.model';

export interface OrderItem {
  id: number;
  orderId: number;
  order?: Order;
  itemId: number;
  item?: ItemModel;
  quantity: number;
  price: number;
}
