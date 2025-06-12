import { Computer_ComputerPart } from "./computer_ComputerPart";
import { PartGroup } from "./partGroup";

/*
*This is the object structure of the computerPart object.
*It is used to store it in the database from frontend to backend. 
*/

export interface ComputerPart {
    id: number,
    partGroupId: number,
    serialNumber: string,
    modelNumber: string,
    // *get the partGroup object from the partGroup.ts file
    group?: PartGroup,
    // *get the computer_ComputerPart object from the computer_ComputerPart.ts file
    computer_ComputerPart?: Computer_ComputerPart[]
}
