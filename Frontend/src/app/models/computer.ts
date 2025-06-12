import { Computer_ComputerPart } from "./computer_ComputerPart";
import { Item } from "./item";

/*
*This is the object structure of the computer object.
*It is used to store it in the database from frontend to backend.
*/

export interface Computer {
    id: number, 
    // *get the item object from the item.ts file
    item?: Item,
    // *get the computer_ComputerPart object from the computer_ComputerPart.ts file
    computer_ComputerParts?: Computer_ComputerPart[]
}