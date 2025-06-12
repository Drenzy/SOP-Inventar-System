import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NavbarComponent } from '../navbar/navbar.component';
import { Request } from '../models/request';
import { RequestService } from '../services/request.service';
import { User } from '../models/user';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-request',
  standalone: true,
  imports: [CommonModule, FormsModule, NavbarComponent],
  templateUrl: './request.component.html',
  styleUrl: './request.component.css',
})
export class RequestComponent {
  requests: Request[] = [];
  users: User[] = [];
  instuctors: User[] = [];
  newRequest: Request = {
    id: 0,
    userId: 0,
    recipientEmail: '',
    item: '',
    message: '',
    date: new Date(),
    status: '',
    requestUser: {
      id: 0,
      roleId: 0,
      name: '',
      email: '',
      password: '',
      twoFactorAuthentication: true,
      role: { id: 0, name: '', description: '' },
      userLoans: [],
    },
  };
  selectedRequest: Request | null = null;
  archiveRequest: Request | null = null;
  searchRequest: string = '';
  filteredRequest: Request[] = [];

  currentUser: any;
  archiveNote: any;
  showErrorNote: boolean = false;

  constructor(
    private requestService: RequestService,
    private userService: UserService
  ) { }

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
    this.currentUser = JSON.parse(
      localStorage.getItem('currentUser') as string
    );
    this.getRequests();
    this.getUsers();
    this.getInstuctor();
  }

  selectRequest(request: Request): void {
    this.selectedRequest = { ...request };
    this.newRequest = { ...request };
  }

  getRequests(): void {
    this.requestService.getAll().subscribe(
      (data) => {
        this.requests = data;
        this.filterRequest();
      },
      (error: any) => {
        console.error('Error fetching requests', error);
        this.handleError(error);
      }
    );
  }

  getUsers(): void {
    this.userService.getAll().subscribe(
      (users) => {
        this.users = users;
      },
      (error: any) => {
        console.error('Error fetching users', error);
      }
    );
  }

  getInstuctor(): void {
    this.userService.getAll().subscribe(
      (users) => {
        this.instuctors = users.filter(
          (user) => user.roleId === 1 || user.roleId === 2
        );
      },
      (error: any) => {
        console.error('Error fetching instructors', error);
        this.handleError(error);
      }
    );
  }

  filterRequest(): void {
    this.filteredRequest = this.requests.filter(
      (request) =>
        request.item.toLowerCase().includes(this.searchRequest.toLowerCase()) ||
        request.recipientEmail
          .toLowerCase()
          .includes(this.searchRequest.toLowerCase()) ||
        request.requestUser?.email.toLowerCase().includes(this.searchRequest.toLowerCase())
    );
  }

  updateRequest(): void {
    if (!this.selectedRequest) {
      console.error('No request selected for update');
      return;
    }
    this.requestService.update(this.newRequest).subscribe(
      (response) => {
        const index = this.requests.findIndex((req) => req.id === response.id);
        if (index !== -1) {
          this.requests[index] = response;
        }
        this.resetForm();
        window.location.reload();
      },
      (error) => {
        console.error('Error updating request', error);
        this.handleError(error);
      }
    );
  }

  openArchiveModal(request: Request): void {
    this.archiveRequest = request;
    const modal = document.getElementById('ArhciveModal');
    if (modal) modal.style.display = 'block';
  }

  confirmArchiveRequest(): void {
    if (!this.archiveNote?.trim()) {
      this.showErrorNote = true;
      return;
    }

    if (!this.archiveRequest?.id) {
      return;
    }

    this.requestService
      .delete(this.archiveRequest.id, this.archiveNote)
      .subscribe({
        next: () => {
          this.getUsers();
          this.archiveRequest = null;
          this.closeArchiveModal();
          window.location.reload();
        },
        error: (error) => {
          this.handleError(error);
        },
      });
  }

  closeArchiveModal(): void {
    const modal = document.getElementById('ArhciveModal');
    if (modal) modal.style.display = 'none';
    this.archiveNote = '';
    this.showErrorNote = false;
    this.archiveRequest = null;
  }

  resetForm(): void {
    this.newRequest = {
      id: 0,
      userId: 0,
      recipientEmail: '',
      item: '',
      message: '',
      date: new Date(),
      status: '',
      requestUser: {
        id: 0,
        roleId: 0,
        name: '',
        email: '',
        password: '',
        twoFactorAuthentication: true,
        role: { id: 0, name: '', description: '' },
        userLoans: [],
      },
    };
    this.selectedRequest = null;
  }
}
