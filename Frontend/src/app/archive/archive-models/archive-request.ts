export interface ArchiveRequest {
  id: number;
  userId: number;
  recipientEmail: string;
  item: string;
  message: string;
  date: Date;
  status: string;
  deleteTime: Date;
  archiveNote: string;
}
