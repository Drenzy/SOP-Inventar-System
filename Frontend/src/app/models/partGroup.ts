import { PartType } from "./partType";

/*
* This interface represents the PartGroup model.
* It contains the properties of the PartGroup model.
*/
export interface PartGroup {
    id: number,
    partName: string,
    price: number,
    manufacturer: string,
    warrantyPeriod: string,
    releaseDate: Date,
    quantity: number,
    partTypeId: number,
    //* get the partType object from the partType.ts file
    partType: PartType,
}
