import { Country } from './country';
import { Address } from './address';

export interface City {
  id: number;
  name: string;
  countryId: number;
  country?: Country;
  addresses: Address[];
}
