import { ItemType } from './itemType';

export interface ItemGroup {
  id: number;
  itemTypeId: number;
  modelName: string;
  quantity: number;
  price: number;
  manufacturer: string;
  warrantyPeriod: string;
  itemType?: ItemType;
}
