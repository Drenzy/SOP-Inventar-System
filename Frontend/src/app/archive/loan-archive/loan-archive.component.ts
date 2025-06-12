import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from '../../navbar/navbar.component';
import { ArchiveLoan } from '../archive-models/archive-loan';
import { ArchiveLoanService } from '../archive-services/archive-loan.service';
import { UserService } from '../../services/user.service';
import { RouterModule } from '@angular/router';
import { User } from '../../models/user';
import { ArchiveUserService } from '../archive-services/archive-user.service';
import { Item } from '../../models/item';
import { ItemService } from '../../services/item.service';
import { ArchiveItem } from '../archive-models/archive-item';
import { ArchiveItemService } from '../archive-services/archive-item.service';
import { ItemGroup } from '../../models/itemGroup';
import { ItemGroupService } from '../../services/itemGroup.service';
import { ArchiveItemGroupService } from '../archive-services/archive-itemgroup.service';
import { ItemType } from '../../models/itemType';
import { ItemTypeService } from '../../services/itemType.service';
import { ArchiveItemTypeService } from '../archive-services/archive-itemtype.service';

@Component({
  selector: 'app-loan-archive',
  imports: [FormsModule, CommonModule, NavbarComponent, RouterModule],
  templateUrl: './loan-archive.component.html',
  styleUrl: './loan-archive.component.css',
})
export class LoanArchiveComponent implements OnInit {
  archiveLoans: ArchiveLoan[] = [];
  filteredArchivedLoans: ArchiveLoan[] = [];
  searchArchivedLoanTerm: string = '';

  users: User[] = [];
  items: Item[] = [];
  archivedItems: ArchiveItem[] = [];
  itemGroups: ItemGroup[] = [];
  itemTypes: ItemType[] = [];

  constructor(
    private archiveLoanService: ArchiveLoanService,
    private userService: UserService,
    private archiveUserService: ArchiveUserService,
    private itemService: ItemService,
    private archiveItemService: ArchiveItemService,
    private itemGroupService: ItemGroupService,
    private archiveItemGroupService: ArchiveItemGroupService,
    private itemTypeService: ItemTypeService,
    private archiveItemTypeService: ArchiveItemTypeService
  ) { }

  ngOnInit(): void {
    this.fetchLoans();
    this.fetchUsers();
    this.fetchItems();
    this.fetchItemGroups();
    this.fetchItemTypes();
  }

  // USER RELATED METHODS
  fetchUsers(): void {
    this.userService.getAll().subscribe({
      next: (users) => {
        this.users = users;
        this.fetchArchivedUsers();
      },
      error: (error) => {
        console.error('Error fetching users:', error);
        this.fetchArchivedUsers();
      },
    });
  }

  private fetchArchivedUsers(): void {
    this.archiveUserService.getAll().subscribe({
      next: (archivedUsers) => {
        const existingIds = new Set(this.users.map((user) => user.id));
        for (const archivedUser of archivedUsers) {
          if (!existingIds.has(archivedUser.id)) {
            this.users.push(archivedUser);
          }
        }
      },
      error: (error) => console.error('Error fetching archived users:', error),
    });
  }

  getUserEmail(userId: number): string {
    const user = this.users.find((user) => user.id === userId);

    if (user) {
      const email = user.email;
      return 'deleteTime' in user ? `${email} (Arkiveret)` : email;
    }
    return 'Bruger ikke fundet';
  }

  // ITEM RELATED METHODS
  fetchItems(): void {
    this.itemService.getAll().subscribe({
      next: (items) => {
        this.items = items;
        this.fetchArchivedItems();
      },
      error: (error) => {
        console.error('Error fetching items:', error);
        this.fetchArchivedItems();
      },
    });
  }

  private fetchArchivedItems(): void {
    this.archiveItemService.getAll().subscribe({
      next: (archivedItems) => {
        this.archivedItems = archivedItems;
      },
      error: (error) => console.error('Error fetching archived items:', error),
    });
  }

  getItemSerialNumber(itemId: number): string {
    // First check active items
    const item = this.items.find((item) => item.id === itemId);
    if (item) {
      return item.serialNumber;
    }

    // Then check archived items
    const archivedItem = this.archivedItems.find((item) => item.id === itemId);
    if (archivedItem) {
      return `${archivedItem.serialNumber} (Arkiveret)`;
    }

    return 'Ikke fundet';
  }

  getItemGroupName(itemId: number): string {
    // First find the item to get its itemGroupId
    const item = this.items.find((item) => item.id === itemId);
    const archivedItem = item
      ? null
      : this.archivedItems.find((item) => item.id === itemId);

    let itemGroupId: number;

    if (item) {
      itemGroupId = item.itemGroupId;
    } else if (archivedItem) {
      itemGroupId = archivedItem.itemGroupId;
    } else {
      return 'Ikke fundet';
    }

    // Now find the item group
    const itemGroup = this.itemGroups.find((group) => group.id === itemGroupId);

    if (!itemGroup) {
      return 'Gruppe ikke fundet';
    }

    const isArchived = 'deleteTime' in itemGroup;
    return isArchived
      ? `${itemGroup.modelName} (Arkiveret)`
      : itemGroup.modelName;
  }

  getItemTypeName(itemId: number): string {
    // First find the item to get its itemGroupId
    const item = this.items.find((item) => item.id === itemId);
    const archivedItem = item
      ? null
      : this.archivedItems.find((item) => item.id === itemId);

    let itemGroupId: number;

    if (item) {
      itemGroupId = item.itemGroupId;
    } else if (archivedItem) {
      itemGroupId = archivedItem.itemGroupId;
    } else {
      return 'Ikke fundet';
    }

    // Now find the item group to get its itemTypeId
    const itemGroup = this.itemGroups.find((group) => group.id === itemGroupId);

    if (!itemGroup) {
      return 'Type ikke fundet';
    }

    const itemTypeId = itemGroup.itemTypeId;

    // Now find the item type
    const itemType = this.itemTypes.find((type) => type.id === itemTypeId);

    if (!itemType) {
      return 'Type ikke fundet';
    }

    const isArchived = 'deleteTime' in itemType;
    return isArchived ? `${itemType.typeName} (Arkiveret)` : itemType.typeName;
  }

  // ITEM GROUP RELATED METHODS
  fetchItemGroups(): void {
    this.itemGroupService.getAll().subscribe({
      next: (itemGroups) => {
        this.itemGroups = itemGroups;
        this.fetchArchivedItemGroups();
      },
      error: (error) => {
        console.error('Error fetching item groups:', error);
        this.fetchArchivedItemGroups();
      },
    });
  }

  private fetchArchivedItemGroups(): void {
    this.archiveItemGroupService.getAll().subscribe({
      next: (archivedGroups) => {
        const existingIds = new Set(this.itemGroups.map((group) => group.id));
        for (const archivedGroup of archivedGroups) {
          if (!existingIds.has(archivedGroup.id)) {
            this.itemGroups.push(archivedGroup);
          }
        }
      },
      error: (error) =>
        console.error('Error fetching archived item groups:', error),
    });
  }

  // ITEM TYPE RELATED METHODS
  fetchItemTypes(): void {
    this.itemTypeService.getAll().subscribe({
      next: (itemTypes) => {
        this.itemTypes = itemTypes;
        this.fetchArchivedItemTypes();
      },
      error: (error) => {
        console.error('Error fetching item types:', error);
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
        console.error('Error fetching archived item types:', error),
    });
  }

  // LOAN RELATED METHODS
  fetchLoans(): void {
    this.archiveLoanService.getAll().subscribe({
      next: (loans) => {
        this.archiveLoans = loans;
        this.filteredArchivedLoans = [...loans];
      },
      error: (error) => console.error('Error fetching archive loans:', error),
    });
  }

  filterArchiveLoans(): void {
    const searchTerm = this.searchArchivedLoanTerm.toLowerCase().trim();

    if (!searchTerm) {
      this.filteredArchivedLoans = [...this.archiveLoans];
      return;
    }

    this.filteredArchivedLoans = this.archiveLoans.filter((loan) => {
      const userEmail = this.getUserEmail(loan.userId).toLowerCase();
      const serialNumber = this.getItemSerialNumber(loan.itemId).toLowerCase();
      const modelName = this.getItemGroupName(loan.itemId).toLowerCase();
      const typeName = this.getItemTypeName(loan.itemId).toLowerCase();

      return (
        userEmail.includes(searchTerm) ||
        serialNumber.includes(searchTerm) ||
        modelName.includes(searchTerm) ||
        typeName.includes(searchTerm) ||
        loan.id.toString().includes(searchTerm)
      );
    });
  }

  confirmDelete(id: number): void {
    if (confirm('Er du sikker på at du vil slette dette lån permanent?')) {
      this.archiveLoanService.delete(id).subscribe({
        next: () => {
          this.archiveLoans = this.archiveLoans.filter(
            (loan) => loan.id !== id
          );
          this.fetchLoans(); // Better than window.location.reload()
        },
        error: (error) => console.error('Error deleting archive loan:', error),
      });
    }
  }

  restoreLoan(id: number): void {
    if (confirm('Er du sikker på at du vil gendanne dette lån?')) {
      const loan = this.archiveLoans.find((l) => l.id === id);

      if (!loan) {
        alert('Lånet blev ikke fundet.');
        return;
      }

      // Check if user exists (either active or archived)
      const userExists = !!this.users.find((user) => user.id === loan.userId);
      if (!userExists) {
        alert('Brugeren findes ikke længere. Lånet kan ikke gendannes.');
        return;
      }

      // Check if user is active (not archived)
      const userIsActive = this.users.find(
        (user) => user.id === loan.userId && !('deleteTime' in user)
      );
      if (!userIsActive) {
        alert(
          'Brugeren er arkiveret. Gendan brugeren først før lånet kan gendannes.'
        );
        return;
      }

      // Check if item exists (active only - archived items should not be loaned)
      const itemExists = this.items.find((item) => item.id === loan.itemId);
      if (!itemExists) {
        alert(
          'Genstanden findes ikke længere eller er arkiveret. Lånet kan ikke gendannes.'
        );
        return;
      }

      // All checks passed, proceed with restore
      this.archiveLoanService.restore(id).subscribe({
        next: () => {
          this.archiveLoans = this.archiveLoans.filter(
            (loan) => loan.id !== id
          );
          this.fetchLoans();
        },
        error: (error) => {
          console.error('Error restoring loan:', error);
          alert('Der opstod en fejl ved gendannelse af lånet.');
        },
      });
    }
  }
}
