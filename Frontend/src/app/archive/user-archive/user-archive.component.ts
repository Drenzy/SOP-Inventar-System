import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from '../../navbar/navbar.component';
import { ArchiveUser } from '../archive-models/archive-user';
import { ArchiveUserService } from '../archive-services/archive-user.service';
import { RouterModule } from '@angular/router';
import { Role } from '../../models/role';
import { RoleService } from '../../services/role.service';

@Component({
  selector: 'app-user-archive',
  imports: [FormsModule, CommonModule, NavbarComponent, RouterModule],
  templateUrl: './user-archive.component.html',
  styleUrl: './user-archive.component.css',
})
export class UserArchiveComponent implements OnInit {
  archiveUsers: ArchiveUser[] = [];
  filteredArchivedUser: ArchiveUser[] = [];
  searchArchivedUser: string = '';
  roles: Role[] = [];

  constructor(
    private archiveUserService: ArchiveUserService,
    private roleService: RoleService
  ) { }

  ngOnInit(): void {
    this.fetchUser();
    this.fetchRoles();
  }

  fetchRoles(): void {
    this.roleService.getAll().subscribe({
      next: (roles) => {
        this.roles = roles;
      },
      error: (error) => {
        console.error('Error fetching roles:', error);
      },
    });
  }

  getRoleName(roleId: number): string {
    const role = this.roles.find((role) => role.id === roleId);

    if (role) {
      const roleName = role.name;

      return roleName;
    }
    return 'Rolle ikke fundet';
  }

  fetchUser(): void {
    this.archiveUserService.getAll().subscribe({
      next: (user) => {
        this.archiveUsers = user;
        this.filteredArchivedUser = [...user];
      },
      error: (error) => {
        console.error('Error fetching archive users:', error);
      },
    });
  }

  filteredArchivedUsers(): void {
    const searchTerm = this.searchArchivedUser.toLowerCase().trim();

    if (!searchTerm) {
      this.filteredArchivedUser = [...this.archiveUsers];
      return;
    }

    this.filteredArchivedUser = this.archiveUsers.filter((user) => {
      // Get role name to include in search
      const roleName = this.getRoleName(user.roleId).toLowerCase();

      return (
        user.name.toLowerCase().includes(searchTerm) ||
        user.email.toLowerCase().includes(searchTerm) ||
        roleName.includes(searchTerm)
      );
    });
  }

  confirmDelete(id: number): void {
    if (confirm('Er du sikker på at du vil slette denne bruger permanent?')) {
      this.archiveUserService.delete(id).subscribe({
        next: () => {
          this.archiveUsers = this.archiveUsers.filter(
            (user) => user.id !== id
          );
          this.fetchUser();
        },
        error: (error) => console.error('Error deleting archive user:', error),
      });
    }
  }

  restoreUser(id: number): void {
    if (confirm('Er du sikker på at du vil gendanne denne bruger?')) {
      this.archiveUserService.restore(id).subscribe({
        next: () => {
          this.archiveUsers = this.archiveUsers.filter(
            (user) => user.id !== id
          );
          this.fetchUser();
        },
        error: (error) => console.error('Error restoring user:', error),
      });
    }
  }
}
