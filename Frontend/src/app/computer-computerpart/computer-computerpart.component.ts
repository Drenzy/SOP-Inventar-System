import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NavbarComponent } from '../navbar/navbar.component';
import { Computer_ComputerPart } from '../models/computer_ComputerPart';
import { Computer_ComputerPartService } from '../services/computer_ComputerPart.service';
import { ComputerPartService } from '../services/computerPart.service';
import { ComputerPart } from '../models/computerPart';
import { ComputerService } from '../services/computer.service';
import { Computer } from '../models/computer';

@Component({
  selector: 'app-computer-computerpart',
  standalone: true,
  imports: [FormsModule, CommonModule, NavbarComponent],
  templateUrl: './computer-computerpart.component.html',
  styleUrls: ['./computer-computerpart.component.css'],
})
export class ComputerComputerpartComponent implements OnInit {
  // define variables
  computerComputerParts: Computer_ComputerPart[] = [];
  computerParts: ComputerPart[] = [];
  availableComputerParts: ComputerPart[] = [];
  computers: Computer[] = [];
  computerComputerPart: Computer_ComputerPart = {
    id: 0,
    computerId: 0,
    computerPartId: 0,
    computerPart: undefined,
  };
  searchComputer_ComputerPart: string = '';
  filteredComputer_ComputerPart: Computer_ComputerPart[] = [];
  showCreateForm: boolean = false;

  currentUser: any;

  // constructor with services injection
  constructor(
    private computerComputerPartService: Computer_ComputerPartService,
    private computerPartService: ComputerPartService,
    private computerService: ComputerService
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

  // load data on init
  ngOnInit(): void {
    this.currentUser = JSON.parse(
      localStorage.getItem('currentUser') as string
    );

    this.loadComputerComputerParts();
    this.loadComputerParts();
    this.loadComputers();
  }

  // load data methods
  loadComputerComputerParts(): void {
    this.computerComputerPartService.getAll().subscribe(
      (data) => {
        this.computerComputerParts = data;
        this.filterCCP();
      },
      (error) => {
        this.handleError(error);
      }
    );
  }

  // load computer parts
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

  // load computers
  loadComputers(): void {
    this.computerService.getAll().subscribe(
      (data) => {
        this.computers = data;
      },
      (error) => {
        this.handleError(error);
      }
    );
  }

  // filter available computer parts
  filterAvailableComputerParts(): void {
    this.availableComputerParts = this.computerParts.filter(
      (part) =>
        !part.computer_ComputerPart || part.computer_ComputerPart.length === 0
    );
  }

  // filter computer computer parts
  filterCCP(): void {
    this.filteredComputer_ComputerPart = this.computerComputerParts.filter(
      (ccp) =>
        ccp.computerPart?.serialNumber
          .toLowerCase()
          .includes(this.searchComputer_ComputerPart.toLowerCase()) ||
        ccp.computerPart?.modelNumber
          .toLowerCase()
          .includes(this.searchComputer_ComputerPart.toLowerCase())
    );
  }

  // add computer computer part
  addComputerComputerPart(): void {
    this.computerComputerPartService
      .create(this.computerComputerPart)
      .subscribe(
        () => {
          this.loadComputerComputerParts();
          this.computerComputerPart = {
            id: 0,
            computerId: 0,
            computerPartId: 0,
            computerPart: undefined,
          };
          this.loadComputerParts();
          this.showCreateForm = false;
        },
        (error) => {
          this.handleError(error);
        }
      );
  }

  // Confirm delete computer computer part
  confirmDelete(id: number): void {
    if (confirm('Er du sikker på at du vil slette?')) {
      this.delete(id);
    }
  }

  // delete computer computer part
  delete(id: number): void {
    this.computerComputerPartService.delete(id).subscribe(
      () => {
        this.loadComputerComputerParts();
      },
      (error) => {
        this.handleError(error);
      }
    );
  }

  // Sets show create form to false
  cancelCreateForm(): void {
    this.showCreateForm = false;
  }

  // Check available parts
  checkAvailableParts(): void {
    if (this.availableComputerParts.length === 0) {
      alert(
        'There are no available computer parts. Please add new computer parts.'
      );
    } else {
      this.showCreateForm = true;
    }
  }
}
