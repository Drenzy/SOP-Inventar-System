import { User } from './user';
export interface Request {
  id: number;
  userId: number;
  recipientEmail: string;
  item: string;
  message: string;
  date: Date;
  status: string;
  requestUser?: User;
}
