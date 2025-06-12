export interface ArchiveItem {
  id: number;
  roomId: number;
  itemGroupId: number;
  serialNumber: string;
  deleteTime: Date;
  archiveNote: string;
  statusHistories: ArchiveStatusHistory[];
}

export interface ArchiveStatusHistory {
  id: number;
  itemId: number;
  statusId: number;
  statusUpdateDate: Date;
  note: string;
}
