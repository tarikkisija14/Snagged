import { City } from './city';

export interface Country {
  id: number;
  name: string;
  cities: City[];
}
