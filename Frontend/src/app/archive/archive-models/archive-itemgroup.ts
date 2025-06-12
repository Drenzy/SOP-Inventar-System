export interface ArchiveItemGroup {
  id: number;
  itemTypeId: number;
  modelName: string;
  quantity: number;
  price: number;
  manufacturer: string;
  warrantyPeriod: string;
  deleteTime: Date;
  archiveNote: string;
}
