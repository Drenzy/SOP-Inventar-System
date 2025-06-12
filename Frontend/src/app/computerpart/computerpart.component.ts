import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NavbarComponent } from '../navbar/navbar.component';
import { ComputerPart } from '../models/computerPart';
import { PartGroup } from '../models/partGroup';
import { ComputerPartService } from '../services/computerPart.service';
import { PartGroupService } from '../services/partGroup.service';

@Component({
  selector: 'app-computerpart',
  standalone: true,
  imports: [CommonModule, FormsModule, NavbarComponent],
  templateUrl: './computerpart.component.html',
  styleUrls: ['./computerpart.component.css'],
})
export class ComputerpartComponent implements OnInit {
  // Properties for the component definition
  computerParts: ComputerPart[] = [];
  partGroups: PartGroup[] = [];
  newComputerPart: ComputerPart = {
    id: 0,
    partGroupId: 0,
    serialNumber: '',
    modelNumber: '',
    group: {
      id: 0,
      partName: '',
      price: 0,
      manufacturer: '',
      warrantyPeriod: '',
      releaseDate: new Date(),
      quantity: 0,
      partTypeId: 0,
      partType: { id: 0, partTypeName: '' },
    },
    computer_ComputerPart: [],
  };
  selectedComputerPart: ComputerPart | null = null;
  searchComputerPart: string = '';
  filteredComputerParts: ComputerPart[] = [];

  currentUser: any;

  // Constructor for the component definition
  constructor(
    private computerPartService: ComputerPartService,
    private partGroupService: PartGroupService
  ) {}

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

  // Method to initialize the component
  ngOnInit(): void {
    this.currentUser = JSON.parse(
      localStorage.getItem('currentUser') as string
    );
    this.loadComputerParts();
    this.loadPartGroups();
  }

  // Method to load computer parts
  loadComputerParts(): void {
    this.computerPartService.getAll().subscribe(
      (data) => {
        this.computerParts = data;
        this.filterCP();
      },
      (error) => {
        this.handleError(error);
      }
    );
  }

  // Method to load part groups
  loadPartGroups(): void {
    this.partGroupService.getAll().subscribe(
      (data) => {
        this.partGroups = data;
      },
      (error) => {
        this.handleError(error);
      }
    );
  }

  // Method to filter computer parts
  filterCP(): void {
    this.filteredComputerParts = this.computerParts.filter(
      (part) =>
        part.serialNumber
          .toLowerCase()
          .includes(this.searchComputerPart.toLowerCase()) ||
        part.modelNumber
          .toLowerCase()
          .includes(this.searchComputerPart.toLowerCase()) ||
        part.group?.partName
          .toLowerCase()
          .includes(this.searchComputerPart.toLowerCase())
    );
  }

  // Method to select a computer part
  selectComputerPart(part: ComputerPart): void {
    this.selectedComputerPart = part;
    this.newComputerPart = { ...part };
  }

  // Method to create a computer part
  createComputerPart(): void {
    this.computerPartService.create(this.newComputerPart).subscribe(
      () => {
      this.loadComputerParts();
      this.newComputerPart = {
        id: 0,
        partGroupId: 0,
        serialNumber: "",
        modelNumber: "",
        group: { id: 0, partName: '', price: 0, manufacturer: '', warrantyPeriod: '', releaseDate: new Date(), quantity: 0, partTypeId: 0, partType: {id: 0, partTypeName: ''} },
        computer_ComputerPart: []
      };
      this.ngOnInit();
    });
  }


  // Method to update a computer part
  updateComputerPart(): void {
    if (this.selectedComputerPart) {
      this.computerPartService.update(this.newComputerPart).subscribe(
        () => {
          this.loadComputerParts();
          this.selectedComputerPart = null;
          this.newComputerPart = {
            id: 0,
            partGroupId: 0,
            serialNumber: '',
            modelNumber: '',
            group: {
              id: 0,
              partName: '',
              price: 0,
              manufacturer: '',
              warrantyPeriod: '',
              releaseDate: new Date(),
              quantity: 0,
              partTypeId: 0,
              partType: { id: 0, partTypeName: '' },
            },
            computer_ComputerPart: [],
          };
          this.ngOnInit();
        },
        (error) => {
          this.handleError(error);
        }
      );
    }
  }

  // Method Confrim before deleting a computer part
  confirmDelete(id: number): void {
    if (confirm('Are you sure you want to delete this computer part?')) {
      this.deleteComputerPart(id);
    }
  }

  // Method to delete a computer part
  deleteComputerPart(id: number): void {
    this.computerPartService.delete(id).subscribe(
      () => {
        this.computerParts = this.computerParts.filter(
          (part) => part.id !== id
        );
        this.ngOnInit();
        this.resetForm();
      },
      (error) => {
        this.handleError(error);
      }
    );
  }

  // Method to reset the form
  resetForm(): void {
    this.newComputerPart = {
      id: 0,
      partGroupId: 0,
      serialNumber: '',
      modelNumber: '',
      group: {
        id: 0,
        partName: '',
        price: 0,
        manufacturer: '',
        warrantyPeriod: '',
        releaseDate: new Date(),
        quantity: 0,
        partTypeId: 0,
        partType: { id: 0, partTypeName: '' },
      },
      computer_ComputerPart: [],
    };
    this.selectedComputerPart = null;
  }
}
