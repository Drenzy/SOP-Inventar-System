import { Role } from './role';
// used to store user data in local storage and to display the user data in the profile component
export interface Login {
  id: number;
  email: string;
  password: string;
  twoFactorAuthentication: boolean;
  token: string;
  role: Role;
}
