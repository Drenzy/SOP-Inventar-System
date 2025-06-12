import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NavbarComponent } from '../navbar/navbar.component';
import { UserService } from '../services/user.service';
import { RoleService } from '../services/role.service';
import { User } from '../models/user';
import { Role } from '../models/role';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [CommonModule, FormsModule, NavbarComponent],
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css'],
})
export class UsersComponent implements OnInit {
  users: User[] = [];
  filteredUsers: User[] = [];
  roles: Role[] = [];
  selectedGroup: number | null = null;
  searchTerm: string = '';
  selectedUser: User | null = null;
  editingUser: User | null = null;
  currentUser: User | null = null;
  confirmPassword: string = '';
  passwordMismatch: boolean = false;
  weakPassword: boolean = false;
  archiveNote: string = '';
  showErrorNote: boolean = false;

  constructor(
    private userService: UserService,
    private roleService: RoleService,
    private router: Router
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
    this.getUsers();
    this.getRoles();
    this.getCurrentUser();
  }

  getCurrentUser(): void {
    const user = localStorage.getItem('currentUser');
    if (user) {
      this.currentUser = JSON.parse(user);
    }
  }

  getUsers(): void {
    this.userService.getAll().subscribe(
      (data: User[]) => {
        this.users = data;
        this.filterUsers();
      },
      (error) => {
        this.handleError(error);
      }
    );
  }

  getRoles(): void {
    this.roleService.getAll().subscribe(
      (data: Role[]) => {
        this.roles = data;
      },
      (error) => {
        console.error('Error fetching roles', error);
      }
    );
  }

  onGroupChange(event: any): void {
    this.selectedGroup = +event.target.value;
    this.filterUsers();
  }

  onSearchChange(): void {
    this.filterUsers();
  }

  filterUsers(): void {
    this.filteredUsers = this.users.filter((user) => {
      const matchesGroup = !this.selectedGroup || user.roleId === this.selectedGroup;
      const matchesSearch =
        !this.searchTerm ||
        user.name.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        user.email.toLowerCase().includes(this.searchTerm.toLowerCase());
      return matchesGroup && matchesSearch;
    });
  }

  navigateToCreateUser(): void {
    this.router.navigate(['/create-user']);
  }

  selectUser(user: User): void {
    this.selectedUser = user;
    this.editingUser = { ...user };
    delete (this.editingUser as any).password; // Remove password field for the user clicked

    this.confirmPassword = '';
    this.passwordMismatch = false;
    this.weakPassword = false;

    if (user.roleId) {
      this.roleService.findById(user.roleId).subscribe(
        (role) => {
          this.selectedUser!.userRole = role;
        },
        (error) => {
          this.handleError(error);
        }
      );
    }
  }

  closeUserDetailPanel(): void {
    this.editingUser = null;
  }

  updateUser(user: User): void {
    const password = user.password;
    const confirmPassword = this.confirmPassword;

    if (password) {
      // Password validation
      const passwordMismatch = password !== confirmPassword;
      const hasUpperCase = /[A-Z]/.test(password);
      const hasLowerCase = /[a-z]/.test(password);
      const hasNumber = /[0-9]/.test(password);
      const hasSpecialChar = /[!@#$%^&*(),-.?":{}|<>]/.test(password);
      const hasMinLength = password?.length >= 15;

      const weakPassword = !(
        hasUpperCase &&
        hasLowerCase &&
        hasNumber &&
        hasSpecialChar &&
        hasMinLength
      );

      this.passwordMismatch = passwordMismatch;
      this.weakPassword = weakPassword;

      if (passwordMismatch || weakPassword) {
        return;
      }

      this.userService.updatePassword(user).subscribe(
        (updatedUser) => {
          const index = this.users.findIndex((u) => u.id === updatedUser.id);
          if (index !== -1) {
            this.users[index] = updatedUser;
          }
          this.getUsers();
          this.editingUser = null;
          alert('Bruger opdateret!');
        },
        (error) => {
          this.handleError(error);
        }
      );
    } else {
      user.password = ''; // prevent sending null
      this.userService.update(user).subscribe(
        (updatedUser) => {
          const index = this.users.findIndex((u) => u.id === updatedUser.id);
          if (index !== -1) {
            this.users[index] = updatedUser;
          }
          this.getUsers();
          this.editingUser = null;
          alert('Bruger opdateret!');
        },
        (error) => {
          this.handleError(error);
        }
      );
    }
  }

  public isDeleteDisabled(user: User | null): boolean {
    // Return true (disabled) if user has loans, handle undefined userLoans
    return !!user && (user.userLoans?.length ?? 0) > 0;
  }

  confirmArchiveUser(): void {
    if (!this.selectedUser?.id) {
      return;
    }

    // Check for loans with null-safe operation
    if (this.selectedUser.userLoans && this.selectedUser.userLoans.length > 0) {
      alert('Kan ikke arkivere, da der er lån tilknyttet denne bruger.');
      return;
    }

    if (!this.archiveNote.trim()) {
      this.showErrorNote = true;
      return;
    }

    this.userService.delete(this.selectedUser.id, this.archiveNote).subscribe({
      next: () => {
        this.getUsers();
        this.selectedUser = null;
        this.editingUser = null;
        this.closeArchiveModal();
      },
      error: (error) => {
        this.handleError(error);
      },
    });
  }

  openArchiveModal(): void {
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

  resetPasswordValidation(): void {
    this.passwordMismatch = false;
    this.weakPassword = false;
  }

  onSave(): void {
    if (this.editingUser) {
      this.updateUser(this.editingUser);
    } else {
      alert('Kunne ikke opdatere bruger!');
    }
  }
}