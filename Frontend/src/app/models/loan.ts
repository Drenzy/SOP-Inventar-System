import { Item } from './item';
import { User } from './user';

export interface Loan {
  id: number;
  userId: number;
  itemId: number;
  loanDate: Date;
  returnDate: Date;
  loanUser?: User;
  loanItem?: Item;
}
