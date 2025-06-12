export interface ArchiveLoan {
  id: number;
  userId: number;
  itemId: number;
  loanDate: string;
  returnDate: string;
  deleteTime: Date;
  archiveNote: string;
}
