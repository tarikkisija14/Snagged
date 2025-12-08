import { Order } from './order';

export interface Payment {
  id: number;
  paymentMethod?: string;
  paidAmount?: number;
  paymentDate?: Date | string;
  orders: Order[];
  stripePaymentIntentId?: string;
  stripeChargeId?: string;
}
