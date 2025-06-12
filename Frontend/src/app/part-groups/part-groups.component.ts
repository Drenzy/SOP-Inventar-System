import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NavbarComponent } from '../navbar/navbar.component';
import { PartGroup } from '../models/partGroup';
import { PartGroupService } from '../services/partGroup.service';
import { PartType } from '../models/partType';
import { PartTypeService } from '../services/partType.service';

@Component({
  selector: 'app-part-groups',
  standalone: true,
  imports: [CommonModule, FormsModule, NavbarComponent],
  templateUrl: './part-groups.component.html',
  styleUrl: './part-groups.component.css',
})
export class PartGroupsComponent implements OnInit {
  // Variables for part groups, filtered part groups, part types, selected part group, new part group, new part type, search part group. \\
  partGroups: PartGroup[] = [];
  filteredPartGroups: PartGroup[] = [];
  partTypes: PartType[] = [];
  selectedPartGroup: PartGroup | null = null;
  newPartGroup: PartGroup = {
    id: 0,
    partName: '',
    price: 0,
    manufacturer: '',
    warrantyPeriod: '',
    releaseDate: new Date(),
    quantity: 0,
    partTypeId: 0,
    partType: {
      id: 0,
      partTypeName: '',
    },
  };
  newPartType: PartType = {
    id: 0,
    partTypeName: '',
  };
  searchPartGroup: string = '';

  constructor(
    private partGroupService: PartGroupService,
    private partTypeService: PartTypeService
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

  ngOnInit(): void {
    this.getPartGroups();
    this.getPartTypes();
  }

  // Method for getting all part groups. \\
  getPartGroups(): void {
    this.partGroupService.getAll().subscribe(
      (partGroups) => {
        this.partGroups = partGroups;
        this.filterPartGroup();
      },
      (error) => {
        console.error('Error fetching part groups', error);
        this.handleError(error);
      }
    );
  }

  // Method for getting all part types. \\
  getPartTypes(): void {
    this.partTypeService.getAll().subscribe(
      (partTypes) => {
        this.partTypes = partTypes;
      },
      (error) => {
        console.error('Error fetching part types', error);
        this.handleError(error);
      }
    );
  }

  filterPartGroup(): void {
    this.filteredPartGroups = this.partGroups.filter((partGroup) =>
      partGroup.partName
        .toLowerCase()
        .includes(this.searchPartGroup.toLowerCase())
    );
  }

  selectPartGroup(partGroup: PartGroup): void {
    this.selectedPartGroup = { ...partGroup };
  }

  // Method for creating a new part group. \\
  createPartGroup(): void {
    this.partGroupService.create(this.newPartGroup).subscribe(
      (partGroup) => {
        this.getPartGroups();
        this.newPartGroup = {
          id: 0,
          partName: '',
          price: 0,
          manufacturer: '',
          warrantyPeriod: '',
          releaseDate: new Date(),
          quantity: 0,
          partTypeId: 0,
          partType: {
            id: 0,
            partTypeName: '',
          },
        };
      },
      (error) => {
        console.error('Error creating part group', error);
        this.handleError(error);
      }
    );
  }

  // Method for updating a part group. \\
  updatePartGroup(): void {
    if (this.selectedPartGroup) {
      this.partGroupService.update(this.selectedPartGroup).subscribe(
        (updatedPartGroup) => {
          const index = this.partGroups.findIndex(
            (partGroup) => partGroup.id === updatedPartGroup.id
          );
          if (index !== -1) {
            this.partGroups[index] = updatedPartGroup;
          }
          this.ngOnInit();
          this.cancelEdit();
        },
        (error) => {
          console.error('Error updating part group', error);
          this.handleError(error);
        }
      );
    }
  }

  cancelEdit(): void {
    this.selectedPartGroup = null;
  }

  openNewItemTypeModal(): void {
    const modal = document.getElementById('newItemTypeModal');
    if (modal) {
      modal.style.display = 'block';
    }
  }

  closeNewItemTypeModal(): void {
    const modal = document.getElementById('newItemTypeModal');
    if (modal) {
      modal.style.display = 'none';
    }
  }

  // Method for creating a new part type. \\
  createNewPartType(): void {
    this.partTypeService.create(this.newPartType).subscribe(
      (partType) => {
        this.getPartTypes();
        this.newPartType = {
          id: 0,
          partTypeName: '',
        };
        this.closeNewItemTypeModal();
      },
      (error) => {
        console.error('Error creating part type', error);
        this.handleError(error);
      }
    );
  }
}
