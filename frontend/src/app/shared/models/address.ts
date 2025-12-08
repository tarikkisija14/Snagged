import { User } from './user';
import { City } from './city';

export interface Address {
  id: number;
  userId: number;
  user?: User;
  cityId: number;
  city?: City;
  street: string;
  lat?: number;
  lng?: number;
}
