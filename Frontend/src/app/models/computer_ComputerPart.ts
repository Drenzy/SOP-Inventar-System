import { Computer } from "./computer";
import { ComputerPart } from "./computerPart";

/*
*This is the object structure of the computer_ComputerPart object.
*It is used to store it in the database from frontend to backend.
*/

export interface Computer_ComputerPart {
    id: number, 
    computerId: number,
    computerPartId: number,
    // *get the computer object from the computer.ts file
    computer?: Computer,
    // *get the computerPart object from the computerPart.ts file
    computerPart?: ComputerPart
}