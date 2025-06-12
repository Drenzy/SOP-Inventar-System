import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NavbarComponent } from '../navbar/navbar.component';
import { Status } from '../models/status';
import { StatusService } from '../services/status.service';

@Component({
  selector: 'app-status',
  standalone: true,
  imports: [CommonModule, FormsModule, NavbarComponent],
  templateUrl: './status.component.html',
  styleUrl: './status.component.css',
})
export class StatusComponent {
  statuss: Status[] = [];
  newStatus: Status = {
    id: 0,
    name: '',
  };
  selectedStatus: Status | null = null;
  searchStatus: string = '';
  filteredStatuses: Status[] = [];

  constructor(private statusService: StatusService) { }

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
    this.getStatus();
  }

  // Method to get all status. \\
  getStatus(): void {
    this.statusService.getAll().subscribe(
      (status) => {
        this.statuss = status;
        this.filterStatus();
      },
      (error) => {
        this.handleError(error);
      }
    );
  }

  filterStatus(): void {
    this.filteredStatuses = this.statuss.filter((status) =>
      status.name.toLowerCase().includes(this.searchStatus.toLowerCase())
    );
  }

  createStatus(): void {
    this.statusService.create(this.newStatus).subscribe(
      () => {
        this.getStatus();
        this.newStatus = { id: 0, name: '' };
      },
      (error) => {
        console.error('Error creating role', error);
        this.handleError(error);
      }
    );
  }
}
