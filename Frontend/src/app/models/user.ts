import { Role } from './role';

export interface User {
  id: number;
  name: string;
  email: string;
  password: string;
  twoFactorAuthentication: boolean;
  roleId: number;
  userRole?: Role; // needed to display the role in profile component
  role?: Role;
  userLoans?: UserLoan[];
}

// needed to display users data in the profile component
export interface UserLoan {
  id: number;
  loanDate: Date;
  returnDate: Date;
  itemId: number;
  userId: number;
  userLoanItem?: UserLoanItem;
}

// needed to display the items data in the profile component
export interface UserLoanItem {
  id: number;
  roomId: number;
  itemGroupId: number;
  serialNumber: string;
  userLoanItemItemGroup?: UserLoanItemItemGroup;
}

// needed to display the itemgroup data in the profile component
export interface UserLoanItemItemGroup {
  id: number;
  itemTypeId: number;
  modelName: string;
  quantity: number;
  price: number;
  manufacturer: string;
  warrantyPeriod: string;
}
