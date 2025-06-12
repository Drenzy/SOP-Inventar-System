import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NavbarComponent } from '../../navbar/navbar.component';
import { ArchiveItemType } from '../archive-models/archive-itemtype';
import { ArchiveItemTypeService } from '../archive-services/archive-itemtype.service';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-itemtype-archive',
  imports: [FormsModule, CommonModule, NavbarComponent, RouterModule],
  templateUrl: './itemtype-archive.component.html',
  styleUrl: './itemtype-archive.component.css',
})
export class ItemtypeArchiveComponent implements OnInit {
  archiveItemTypes: ArchiveItemType[] = [];

  filteredArchivedItemTypes: ArchiveItemType[] = [];

  searchArchivedItemTypeTerm: string = '';
  constructor(private archiveItemTypeService: ArchiveItemTypeService) { }
  ngOnInit(): void {
    this.fetchItemTypes();
  }

  fetchItemTypes(): void {
    this.archiveItemTypeService.getAll().subscribe({
      next: (itemTypes) => {
        this.archiveItemTypes = itemTypes;
        this.filteredArchivedItemTypes = [...itemTypes];
      },
      error: (error) => {
        console.error('Error fetching archive item types:', error);
      },
    });
  }

  filterArchivedItemTypes(): void {
    this.filteredArchivedItemTypes = this.archiveItemTypes.filter(
      (itemType) => {
        return itemType.typeName.toLowerCase().includes(this.searchArchivedItemTypeTerm);
      }
    );
  }

  confirmDelete(id: number) {
    if (confirm('Er du sikker på at du vil slette denne genstand permanent?')) {
      this.archiveItemTypeService.delete(id).subscribe({
        next: () => {
          this.archiveItemTypes = this.archiveItemTypes.filter(
            (itemtype) => itemtype.id !== id
          );
          window.location.reload();
        },
        error: (error) => {
          console.error('Error deleting archive item:', error);
        },
      });
    }
  }

  restoreItemType(id: number) {
    if (confirm('Er du sikker på at du vil gendanne denne anmodning?')) {
      this.archiveItemTypeService.restore(id).subscribe({
        next: () => {
          this.archiveItemTypes = this.archiveItemTypes.filter(
            (itemtype) => itemtype.id !== id
          );
          window.location.reload();
        },
        error: (error) => {
          console.error('Error deleting archive item:', error);
        },
      });
    }
  }
}
