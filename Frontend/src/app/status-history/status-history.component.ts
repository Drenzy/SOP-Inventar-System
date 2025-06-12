import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from '../navbar/navbar.component';
import { StatusHistoryService } from '../services/statusHistory.service';
import { ItemService } from '../services/item.service';
import { StatusService } from '../services/status.service';
import { StatusHistory } from '../models/statusHistory';
import { Item } from '../models/item';
import { Status } from '../models/status';

@Component({
  selector: 'app-status-history',
  standalone: true,
  imports: [FormsModule, CommonModule, NavbarComponent],
  templateUrl: './status-history.component.html',
  styleUrls: ['./status-history.component.css'],
})
export class StatusHistoryComponent implements OnInit {
  // Creating properties for status histories, items, statuss, and the new status history. \\
  statusHistories: StatusHistory[] = [];
  items: Item[] = [];
  statuss: Status[] = [];
  newStatusHistory: StatusHistory = {
    id: 0,
    itemId: 0,
    statusId: 0,
    statusUpdateDate: new Date(),
    note: '',
    status: undefined,
    item: undefined,
  };
  isEditing: boolean = false;

  constructor(
    private statusHistoryService: StatusHistoryService,
    private itemService: ItemService,
    private statusService: StatusService
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
    this.loadStatusHistories();
    this.loadItems();
    this.loadStatuss();
  }

  loadStatusHistories(): void {
    this.statusHistoryService.getAll().subscribe(
      (data) => {
        this.statusHistories = data;
      },
      (error) => {
        console.error('Error fetching status histories', error);
        this.handleError(error);
      }
    );
  }

  // Method to get all items. \\
  loadItems(): void {
    this.itemService.getAll().subscribe(
      (data) => {
        this.items = data;
      },
      (error) => {
        console.error('Error fetching items', error);
        this.handleError(error);
      }
    );
  }

  // Method to get all statuss. \\
  loadStatuss(): void {
    this.statusService.getAll().subscribe(
      (data) => {
        this.statuss = data;
      },
      (error) => {
        console.error('Error fetching status', error);
        this.handleError(error);
      }
    );
  }

  selectStatusHistory(statusHistory: StatusHistory): void {
    this.newStatusHistory = { ...statusHistory };
    this.isEditing = true;
  }

  // Method to create a new status history. \\
  createStatusHistory(): void {
    this.statusHistoryService.create(this.newStatusHistory).subscribe(() => {
      this.loadStatusHistories();
      this.resetForm();
    },
    (error) => {
      console.error('Error creating status history', error);
      this.handleError(error);
    });
  }

  // Method to update a status history. \\
  updateStatusHistory(): void {
    if (this.isEditing) {
      this.statusHistoryService.update(this.newStatusHistory).subscribe(() => {
        this.loadStatusHistories();
        this.resetForm();
      },
      (error) => {
        console.error('Error updating status history', error);
        this.handleError(error);
      });
    }
  }

  // Method to delete a status history. \\
  resetForm(): void {
    this.newStatusHistory = {
      id: 0,
      itemId: 0,
      statusId: 0,
      statusUpdateDate: new Date(),
      note: '',
      status: undefined,
      item: undefined,
    };
    this.isEditing = false;
  }

  cancelEdit(): void {
    this.resetForm();
  }
}
