import { Item } from './item';
import { Status } from './status';
export interface StatusHistory {
  id: number;
  itemId: number;
  statusId: number;
  statusUpdateDate: Date;
  note: string;
  status?: Status;
  item?: Item;
}
