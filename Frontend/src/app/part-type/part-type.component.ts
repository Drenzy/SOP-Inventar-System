import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from '../navbar/navbar.component';
import { PartTypeService } from '../services/partType.service';
import { PartType } from '../models/partType';

@Component({
  selector: 'app-part-type',
  standalone: true,
  imports: [FormsModule, CommonModule, NavbarComponent],
  templateUrl: './part-type.component.html',
  styleUrl: './part-type.component.css',
})
export class PartTypeComponent implements OnInit {
  partTypes: PartType[] = [];
  filteredPartTypes: PartType[] = [];
  selectedPartType: PartType | null = null;
  newPartType: PartType = {
    id: 0,
    partTypeName: '',
  };
  searchPartType: string = '';

  constructor(private partTypeService: PartTypeService) {}

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
    this.getAllPartTypes();
  }

  // Method for getting all part types. \\
  getAllPartTypes(): void {
    this.partTypeService.getAll().subscribe(
      (partTypes) => {
        this.partTypes = partTypes;
        this.filterPT();
      },
      (error) => {
        console.error('Error fetching part types', error);
        this.handleError(error);
      }
    );
  }

  // Method for filtering part types. \\
  filterPT(): void {
    this.filteredPartTypes = this.partTypes.filter((partType) =>
      partType.partTypeName
        .toLowerCase()
        .includes(this.searchPartType.toLowerCase())
    );
  }

  selectPartType(partType: PartType): void {
    this.selectedPartType = { ...partType };
  }

  // Method for creating a new part type. \\
  createPartType(): void {
    this.partTypeService.create(this.newPartType).subscribe(
      () => {
        this.getAllPartTypes();
        this.newPartType = {
          id: 0,
          partTypeName: '',
        };
      },
      (error) => {
        console.error('Error creating part type', error);
        this.handleError(error);
      }
    );
  }

  // Method for updating a part type. \\
  updatePartType(): void {
    if (this.selectedPartType) {
      this.partTypeService
        .update(this.selectedPartType)
        .subscribe((updatedPartType) => {
          const index = this.partTypes.findIndex(
            (partType) => partType.id === updatedPartType.id
          );
          if (index !== -1) {
            this.partTypes[index] = updatedPartType;
          }
          this.filterPT();
          this.cancelEdit();
        },
        (error) => {
          console.error('Error updating part type', error);
          this.handleError(error);
        });
    }
  }

  cancelEdit(): void {
    this.selectedPartType = null;
  }
}
