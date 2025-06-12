import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NavbarComponent } from '../../navbar/navbar.component';
import { ArchiveRequestService } from '../archive-services/archive-request.service';
import { ArchiveRequest } from '../archive-models/archive-request';
import { UserService } from '../../services/user.service';
import { RouterModule } from '@angular/router';
import { User } from '../../models/user';
import { ArchiveUserService } from '../archive-services/archive-user.service';

@Component({
  selector: 'app-request-archive',
  imports: [FormsModule, CommonModule, NavbarComponent, RouterModule],
  templateUrl: './request-archive.component.html',
  styleUrl: './request-archive.component.css',
})
export class RequestArchiveComponent implements OnInit {
  archiveRequests: ArchiveRequest[] = [];
  filteredArchivedRequest: ArchiveRequest[] = [];
  searchArchivedRequest = '';
  users: User[] = [];

  constructor(
    private archiveRequestService: ArchiveRequestService,
    private userService: UserService,
    private archiveUserService: ArchiveUserService
  ) { }

  ngOnInit(): void {
    this.fetchRequest();
    this.fetchUsers();
  }

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
        // Add archived users to the users array, avoiding duplicates
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
      if ('deleteTime' in user) {
        return `${email} (Arkiveret)`;
      } else {
        return email;
      }
    } else {
      return 'Bruger ikke fundet';
    }
  }


  fetchRequest(): void {
    this.archiveRequestService.getAll().subscribe({
      next: (request) => {
        this.archiveRequests = request;
        this.filteredArchivedRequest = [...request];
      },
      error: (error) => {
        console.error('Error fetching archive requests:', error);
      },
    });
  }

  filteredArchivedRequests(): void {
    const searchTerm = this.searchArchivedRequest.toLowerCase().trim();

    if (!searchTerm) {
      this.filteredArchivedRequest = [...this.archiveRequests];
      return;
    }

    this.filteredArchivedRequest = this.archiveRequests.filter((request) => {
      const userEmail = this.getUserEmail(request.userId).toLowerCase();

      return (
        userEmail.includes(searchTerm) ||
        request.recipientEmail.toLowerCase().includes(searchTerm) ||
        request.item.toLowerCase().includes(searchTerm)
      );
    });
  }

  confirmDelete(id: number): void {
    if (
      confirm('Er du sikker på at du vil slette denne anmodning permanent?')
    ) {
      this.archiveRequestService.delete(id).subscribe({
        next: () => {
          this.archiveRequests = this.archiveRequests.filter(
            (request) => request.id !== id
          );
          this.fetchRequest();
        },
        error: (error) =>
          console.error('Error deleting archive Request:', error),
      });
    }
  }

  restoreRequest(id: number): void {
    if (confirm('Er du sikker på at du vil gendanne denne anmodning?')) {
      const request = this.archiveRequests.find((req) => req.id === id);

      if (!request) {
        alert('Anmodningen blev ikke fundet.');
        return;
      }

      this.userService.findById(request.userId).subscribe({
        next: (user) => {
          if (!user) {
            alert(
              'Brugeren findes ikke længere. Anmodningen kan ikke gendannes.'
            );
            return;
          }

          this.archiveRequestService.restore(id).subscribe({
            next: () => {
              this.archiveRequests = this.archiveRequests.filter(
                (req) => req.id !== id
              );
              this.fetchRequest();
            },
            error: (error) => {
              console.error('Error restoring request:', error);
              alert('Der opstod en fejl ved gendannelse af anmodningen.');
            },
          });
        },
        error: (error) => {
          console.error('Error checking user:', error);
          alert(
            'Brugeren findes ikke længere. Anmodningen kan ikke gendannes.'
          );
        },
      });
    }
  }
}
