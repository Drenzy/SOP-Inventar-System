export interface ArchiveUser {
  id: number;
  roleId: number;
  name: string;
  email: string;
  password: string;
  twoFactorAuthentication: boolean;
  deleteTime: Date;
  archiveNote: string;
}
