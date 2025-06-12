import { Address } from './address';

export interface Building {
  id: number;
  buildingName: string;
  zipCode: number;
  buildingAddress?: Address;
}
