import { Building } from './building';
export interface Room {
  id: number;
  buildingId: number;
  roomNumber: number;
  building?: Building;
}
