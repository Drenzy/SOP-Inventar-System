import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RoomService } from '../services/room.service';
import { Room } from '../models/room';
import { BuildingService } from '../services/building.service';
import { Building } from '../models/building';
import { NavbarComponent } from '../navbar/navbar.component';
import { Item } from '../models/item';
import { ItemService } from '../services/item.service';

@Component({
  selector: 'app-rooms',
  standalone: true,
  imports: [CommonModule, FormsModule, NavbarComponent],
  templateUrl: './room.component.html',
  styleUrls: ['./room.component.css'],
})
export class RoomComponent implements OnInit {
  rooms: Room[] = [];
  buildings: Building[] = [];
  filteredRooms: Room[] = [];
  selectedRoom: Room | null = null;
  newRoom: Room = {
    id: 0,
    roomNumber: 0,
    buildingId: 0,
    building: undefined,
  };

  items: Item[] = [];

  searchRoomTerm: string = '';
  showCreateForm: boolean = false;

  currentUser: any;
  remainingTime: number = 0;

  constructor(
    private roomService: RoomService,
    private buildingService: BuildingService,
    private itemService: ItemService
  ) { }

  // Handle errors
  private handleError(error: any): void {
    switch (error.status) {
      case 0:
        alert('Ingen forbindelse til server. Prøv igen senere.');
        break;
      case 400:
        alert('Fejl i forespørgsel til serveren. Prøv igen senere.');
        break;
      case 500:
        alert('Fejl på Serveren. Prøv igen senere.');
        break;
      default:
        alert('En ukendt fejl opstod. Prøv igen senere.');
        break;
    }
  }

  ngOnInit(): void {
    this.currentUser = JSON.parse(
      localStorage.getItem('currentUser') as string
    );

    this.getBuildings();
    this.getAllItems();
  }

  // Fetch all rooms. \\
  getRooms(): void {
    this.roomService.getAll().subscribe(
      (rooms) => {
        this.rooms = rooms.map((room) => {
          room.building = this.buildings.find(
            (building) => building.id === room.buildingId
          );
          return room;
        });
        this.filteredRooms = this.rooms;
      },
      (error) => {
        console.error('Error fetching rooms', error);
        this.handleError(error);
      }
    );
  }

  getAllItems(): void {
    this.itemService.getAll().subscribe({
      next: (items) => {
        this.items = items;
      },
      error: (error) => {
        console.error('Error fetching items', error);
        this.handleError(error);
      },
    });
  }

  // Method for getting all rooms. \\
  filterRooms(): void {
    this.filteredRooms = this.rooms.filter(
      (room) =>
        room.roomNumber
          .toString()
          .includes(this.searchRoomTerm.toLowerCase()) ||
        room.building?.buildingName
          .toLowerCase()
          .includes(this.searchRoomTerm.toLowerCase()) ||
        room.building?.buildingAddress?.road
          .toLowerCase()
          .includes(this.searchRoomTerm.toLowerCase())
    );
  }

  // Method for getting all buildings. \\
  getBuildings(): void {
    this.buildingService.getAll().subscribe(
      (buildings) => {
        this.buildings = buildings;
        this.getRooms(); // Ensure rooms are loaded after buildings
      },
      (error) => {
        console.error('Error fetching buildings', error);
        this.handleError(error);
      }
    );
  }

  selectRoom(room: Room): void {
    this.selectedRoom = { ...room };
  }

  // Method for creating a new room. \\
  createRoom(): void {
    this.roomService.create(this.newRoom).subscribe(() => {
      this.getRooms();
      this.resetForm();
    });
  }

  // Method for updating a room. \\
  updateRoom(room: Room): void {
    this.roomService.update(room).subscribe(
      () => {
        this.getRooms();
        this.selectedRoom = null;
      },
      (error) => {
        console.error('Error updating room', error);
        this.handleError(error);
      }
    );
  }

  // Method for deleting a room. \\
  ConfirmDelete(room: Room): void {
    if (this.items.some((item) => item.roomId === room.id)) {
      alert('Kan ikke slette dette lokale, da der befinder sig en eller flere genstande i den.');
      return;
    }

    const confirmed = confirm(`Vil du gerne slette lokale ${room.roomNumber} i bygning ${room.building?.buildingName}?`);
    if (!confirmed) return;

    this.roomService.delete(room.id).subscribe({
      next: () => this.getRooms(),
      error: (error) => {
        console.error('Error deleting room', error);
        this.handleError(error);
      }
    });
  }

  //* Disable delete button if there are items associated with the room
  public isDeleteDisabled(room: Room): boolean {
    return this.items.some((item) => item.roomId === room.id);
  }

  // Cancel methods for editing and creating. \\
  cancelEdit(): void {
    this.selectedRoom = null;
  }

  cancelCreate(): void {
    this.showCreateForm = false;
    this.resetForm();
  }

  resetForm(): void {
    this.newRoom = {
      id: 0,
      roomNumber: 0,
      buildingId: 0,
      building: undefined,
    };
  }
}
