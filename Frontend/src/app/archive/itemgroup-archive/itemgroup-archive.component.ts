import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NavbarComponent } from '../../navbar/navbar.component';
import { ItemType } from '../../models/itemType';
import { ItemTypeService } from '../../services/itemType.service';
import { ArchiveItemGroup } from '../archive-models/archive-itemgroup';
import { ArchiveItemGroupService } from '../archive-services/archive-itemgroup.service';
import { ArchiveItemTypeService } from '../archive-services/archive-itemtype.service';

@Component({
  selector: 'app-itemgroup-archive',
  imports: [FormsModule, CommonModule, NavbarComponent, RouterModule],
  templateUrl: './itemgroup-archive.component.html',
  styleUrl: './itemgroup-archive.component.css',
})
export class ItemgroupArchiveComponent implements OnInit {
  archiveItemGroup: ArchiveItemGroup[] = [];
  itemTypes: ItemType[] = [];
  filteredArchivedItemGroups: ArchiveItemGroup[] = [];
  searchArchivedItemGroupTerm = '';

  constructor(
    private archiveItemGroupService: ArchiveItemGroupService,
    private archiveItemTypeService: ArchiveItemTypeService,
    private itemTypeService: ItemTypeService
  ) { }

  ngOnInit(): void {
    this.fetchItemGroups();
    this.getItemTypes();
  }

  getItemTypes(): void {
    this.itemTypeService.getAll().subscribe({
      next: (itemTypes) => {
        this.itemTypes = itemTypes;
        this.fetchArchivedItemTypes();
      },
      error: (error) => {
        console.error('Error fetching item types', error);
        this.fetchArchivedItemTypes();
      },
    });
  }

  private fetchArchivedItemTypes(): void {
    this.archiveItemTypeService.getAll().subscribe({
      next: (archivedTypes) => {
        const existingIds = new Set(this.itemTypes.map((type) => type.id));
        for (const archivedType of archivedTypes) {
          if (!existingIds.has(archivedType.id)) {
            this.itemTypes.push(archivedType);
          }
        }
      },
      error: (error) =>
        console.error('Error fetching archived item types', error),
    });
  }

  getItemTypeName(itemTypeId: number): string {
    const itemType = this.itemTypes.find((type) => type.id === itemTypeId);

    if (itemType) {
      const typeName = itemType.typeName;
      if ('deleteTime' in itemType) {
        return `${typeName} (Arkiveret)`;
      } else {
        return typeName;
      }
    } else {
      return 'Gendstandstype ikke fundet';
    }
  }

  fetchItemGroups(): void {
    this.archiveItemGroupService.getAll().subscribe({
      next: (itemGroups) => {
        this.archiveItemGroup = itemGroups;
        this.filteredArchivedItemGroups = [...itemGroups];
      },
      error: (error) =>
        console.error('Error fetching archive itemgroups:', error),
    });
  }
  

  filterArchiveItemGroups(): void {
    this.filteredArchivedItemGroups = this.archiveItemGroup.filter(
      (itemGroup) =>
        itemGroup.modelName.toLowerCase().includes(this.searchArchivedItemGroupTerm) ||
        itemGroup.manufacturer.toLowerCase().includes(this.searchArchivedItemGroupTerm)
    );
  }

  confirmDelete(id: number): void {
    if (confirm('Er du sikker på at du vil slette denne genstand permanent?')) {
      this.archiveItemGroupService.delete(id).subscribe({
        next: () => {
          this.archiveItemGroup = this.archiveItemGroup.filter(
            (item) => item.id !== id
          );
          this.fetchItemGroups();
        },
        error: (error) => console.error('Error deleting archive item:', error),
      });
    }
  }

  restoreItemGroup(id: number): void {
    if (
      confirm('Er du sikker på at du vil gendanne denne gendstands gruppe?')
    ) {
      const itemGroup = this.archiveItemGroup.find((ig) => ig.id === id);

      if (!itemGroup) {
        alert('Genstandsgruppen blev ikke fundet.');
        return;
      }

      this.itemTypeService.findById(itemGroup.itemTypeId).subscribe({
        next: () => {
          this.archiveItemGroupService.restore(id).subscribe({
            next: () => {
              this.archiveItemGroup = this.archiveItemGroup.filter(
                (ig) => ig.id !== id
              );
              this.fetchItemGroups();
            },
            error: (error) => {
              console.error('Error restoring itemgroup:', error);
              alert('Der opstod en fejl ved gendannelse af genstandsgruppen.');
            },
          });
        },
        error: () => {
          alert(
            'Genstandstypen findes ikke længere. Genstandsgruppen kan ikke gendannes.'
          );
        },
      });
    }
  }
}
