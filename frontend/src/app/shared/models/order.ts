import { User } from './user';
import { Payment } from './payment';
import { OrderItem } from './order-item';

export interface Order {
  id: number;
  buyerId: number;
  orderDate: Date | string;
  status: string;
  paymentId?: number;
  buyer?: User;
  payment?: Payment;
  orderItems: OrderItem[];
  stripePaymentIntentId?: string;
}
