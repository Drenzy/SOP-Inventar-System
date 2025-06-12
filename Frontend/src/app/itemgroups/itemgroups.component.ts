import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { NavbarComponent } from '../navbar/navbar.component';
import { FormsModule } from '@angular/forms';
import { ItemGroupService } from '../services/itemGroup.service';
import { ItemGroup } from '../models/itemGroup';
import { ItemType } from '../models/itemType';
import { ItemTypeService } from '../services/itemType.service';
import { Item } from '../models/item';
import { ItemService } from '../services/item.service';

@Component({
  selector: 'app-itemgroups',
  standalone: true,
  imports: [CommonModule, FormsModule, NavbarComponent],
  templateUrl: './itemgroups.component.html',
  styleUrls: ['./itemgroups.component.css'],
})
export class ItemGroupsComponent implements OnInit {
  itemGroups: ItemGroup[] = [];
  itemTypes: ItemType[] = [];
  items: Item[] = [];
  filteredItemGroups: ItemGroup[] = [];

  selectedItemGroup: ItemGroup | null = null;
  tempItemGroup: ItemGroup = this.resetTemp();
  searchItemGroup: string = '';
  archiveNote: string = '';
  showErrorNote: boolean = false;

  constructor(
    private itemTypeService: ItemTypeService,
    private itemGroupService: ItemGroupService,
    private itemService: ItemService
  ) { }

  ngOnInit(): void {
    this.getItemTypes();
    this.getItemGroups();
    this.getAllItems();
  }

  private handleError(error: any): void {
    console.error('Error', error);
    alert('Noget gik galt. Prøv igen senere.');
  }

  getItemTypes(): void {
    this.itemTypeService.getAll().subscribe({
      next: (itemTypes) => (this.itemTypes = itemTypes),
      error: (error) => this.handleError(error),
    });
  }

  getItemGroups(): void {
    this.itemGroupService.getAll().subscribe({
      next: (itemGroups) => {
        this.itemGroups = itemGroups;
        this.filteredItemGroups = itemGroups;
      },
      error: (error) => this.handleError(error),
    });
  }

  // Method for getting all items. \\
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

  filterItemGroup(): void {
    const term = this.searchItemGroup.toLowerCase();
    this.filteredItemGroups = this.itemGroups.filter((ig) =>
      [ig.modelName, ig.manufacturer, ig.itemType?.typeName]
        .some(field => field?.toLowerCase().includes(term))
    );
  }

  editItemGroup(itemGroup: ItemGroup): void {
    this.selectedItemGroup = itemGroup;
    this.tempItemGroup = { ...itemGroup };
  }

  saveItemGroup(): void {
    if (this.selectedItemGroup) {
      this.itemGroupService.update(this.tempItemGroup).subscribe({
        next: () => {
          this.getItemGroups();
          this.cancelEdit();
        },
        error: (error) => this.handleError(error),
      });
    } else {
      this.itemGroupService.create(this.tempItemGroup).subscribe({
        next: () => {
          this.getItemGroups();
          this.tempItemGroup = this.resetTemp();
        },
        error: (error) => this.handleError(error),
      });
    }
  }

  cancelEdit(): void {
    this.selectedItemGroup = null;
    this.tempItemGroup = this.resetTemp();
  }

  private resetTemp(): ItemGroup {
    return {
      id: 0,
      modelName: '',
      itemTypeId: 0,
      quantity: 0,
      price: 0,
      manufacturer: '',
      warrantyPeriod: '',
    };
  }

  confirmArchiveItemGroup(): void {

    if (this.items.some(item => item.itemGroup?.id === this.selectedItemGroup?.id && item.loan !== null)) {
      alert('Kan ikke arkivere, da der er lån tilknyttet denne gruppe.');
      return;
    }

    if (!this.archiveNote.trim()) {
      this.showErrorNote = true;
      return;
    }

    if (!this.selectedItemGroup?.id) {
      return;
    }

    this.itemGroupService.delete(this.selectedItemGroup.id, this.archiveNote).subscribe({
      next: () => {
        this.getItemGroups();
        this.closeArchiveModal();
      },
      error: (error) => {
        this.handleError(error);
      },
    });
  }

  //* Disable delete button if there are loans associated with the item group
  public isDeleteDisabled(itemGroupId: number): boolean {
    return this.items.some(item => item.itemGroup?.id === itemGroupId && item.loan !== null);
  }

  openArchiveModal(itemGroup: ItemGroup): void {
    this.selectedItemGroup = itemGroup;
    const modal = document.getElementById('ArhciveModal');
    if (modal) {
      modal.style.display = 'block';
    }
  }

  closeArchiveModal(): void {
    const modal = document.getElementById('ArhciveModal');
    if (modal) {
      modal.style.display = 'none';
    }
    this.archiveNote = '';
    this.showErrorNote = false;
  }
}
