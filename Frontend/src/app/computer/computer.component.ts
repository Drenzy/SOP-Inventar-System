import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NavbarComponent } from '../navbar/navbar.component';
import { Router } from '@angular/router';
import { ComputerService } from '../services/computer.service';
import { ComputerPartService } from '../services/computerPart.service';
import { Computer_ComputerPartService } from '../services/computer_ComputerPart.service';
import { Computer } from '../models/computer';
import { ComputerPart } from '../models/computerPart';
import { Computer_ComputerPart } from '../models/computer_ComputerPart';
import { Item } from '../models/item';
import { SharedService } from '../services/shared.service';

@Component({
  selector: 'app-computer',
  standalone: true,
  imports: [CommonModule, FormsModule, NavbarComponent],
  templateUrl: './computer.component.html',
  styleUrls: ['./computer.component.css'],
})
export class ComputerComponent implements OnInit {
  // Variables for the component class
  computers: Computer[] = [];
  computerParts: ComputerPart[] = [];
  availableComputerParts: ComputerPart[] = [];

  // Objects for the component class with default values
  computer: Computer = {
    id: 0,
    item: undefined,
    computer_ComputerParts: [],
  };
  items: Item[] = [];

  // Objects for the component class with default values
  newComputer: Computer = {
    id: 0,
    item: undefined,
    computer_ComputerParts: [],
  };

  // Objects for the component class with default values
  newComputerPart: Computer_ComputerPart = {
    id: 0,
    computerId: 0,
    computerPartId: 0,
    computerPart: undefined,
  };

  // Variables for the component class
  isEditing: boolean = false;
  searchComputer: string = '';
  filteredComputer: Computer[] = [];
  selectedComputer: Computer | undefined;
  showCreateForm: boolean = false;

  currentUser: any;

  // Constructor with the services injected as parameters for use in the class
  constructor(
    private computerService: ComputerService,
    private computerPartService: ComputerPartService,
    private computerComputerPartService: Computer_ComputerPartService,
    private router: Router,
    private sharedService: SharedService
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

  // Method that runs when the component is loaded
  ngOnInit(): void {
    this.currentUser = JSON.parse(
      localStorage.getItem('currentUser') as string
    );

    this.loadComputers();
    this.loadComputerParts();
    const computerId = this.sharedService.getComputerId();
    if (computerId) {
      this.selectComputer(computerId);
      this.sharedService.clearComputerId();
    }
  }

  // Method that loads all computers from the database
  loadComputers(): void {
    this.computerService.getAll().subscribe(
      (data) => {
        this.computers = data;
        this.filteredComputer = data;
      },
      (error) => {
        this.handleError(error);
      }
    );
  }

  // Method that loads all computer parts from the database
  loadComputerParts(): void {
    this.computerPartService.getAll().subscribe(
      (data) => {
        this.computerParts = data;
        this.filterAvailableComputerParts();
      },
      (error) => {
        this.handleError(error);
      }
    );
  }

  // Method that filters the available computer parts
  filterAvailableComputerParts(): void {
    this.availableComputerParts = this.computerParts.filter(
      (part) =>
        !part.computer_ComputerPart || part.computer_ComputerPart.length === 0
    );
  }

  // Method that filters the computers based on the search input
  filterComputer(): void {
    this.filteredComputer = this.computers.filter((computer) =>
      computer.item?.serialNumber
        .toLowerCase()
        .includes(this.searchComputer.toLowerCase())
    );
  }

  // Method that gets the selected computer from the table to the form
  selectComputer(computerId: number): void {
    this.computerService.findById(computerId).subscribe(
      (data) => {
        this.selectedComputer = data;
        this.newComputerPart.computerId = computerId;
      },
      (error) => {
        this.handleError(error);
      }
    );
  }

  // Method that adds parts to the selected computer
  addComputerPart(): void {
    if (this.selectedComputer) {
      this.computerComputerPartService.create(this.newComputerPart).subscribe(
        () => {
          this.selectComputer(this.selectedComputer!.id);
          this.showCreateForm = false;
          this.refresh();
        },
        (error) => {
          this.handleError(error);
        }
      );
    }
  }

  // Method for confirming the deletion of an item
  confirmDelete(id: number, type: string): void {
    if (confirm('Are you sure you want to delete this item?')) {
      if (type === 'computer') {
        this.delete(id);
      } else if (type === 'computerPart') {
        this.deleteComputerPart(id);
      }
    }
  }

  // Method that deletes a computer from the database
  delete(computerId: number): void {
    this.computerService.deleteComputerAndItem(computerId).subscribe(
      () => {
        this.loadComputers();
        this.refresh();
      },
      (error) => {
        this.handleError(error);
      }
    );
  }

  // Method that deletes a computer part from the database
  deleteComputerPart(partId: number): void {
    this.computerComputerPartService.delete(partId).subscribe(
      () => {
        if (this.selectedComputer) {
          this.selectComputer(this.selectedComputer.id);
        }
        this.refresh();
      },
      (error) => {
        this.handleError(error);
      }
    );
  }

  // Method that sets the showCreateForm variable to false
  cancelCreateForm(): void {
    this.showCreateForm = false;
  }

  // Method that checks if there are available parts to add to the computer, if not, it will show an alert
  checkAvailableParts(): void {
    if (this.availableComputerParts.length === 0) {
      const userConfirmed = confirm(
        'Der er ikke flere tilgængelige computerdele, venligst tilføj flere dele først på "computer dele" siden, eller fjern fra en eksisterende computer. Vil du fortsætte til "computer dele" siden?'
      );
      if (userConfirmed) {
        this.router.navigate(['/computerPart']);
      }
    } else {
      this.showCreateForm = true;
    }
  }

  // Method that sets the isEditing variable to undefined
  cancelDetail(): void {
    this.selectedComputer = undefined;
  }

  // Method that reloads the window, is used to make sure alerts work properly
  refresh(): void {
    window.location.reload();
  }
}
