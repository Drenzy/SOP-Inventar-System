import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserService } from '../services/user.service';
import { User } from '../models/user';
import { NavbarComponent } from '../navbar/navbar.component';

@Component({
  selector: 'app-users-student',
  templateUrl: './users-student.component.html',
  styleUrls: ['./users-student.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule, NavbarComponent],
})
export class UsersStudentComponent implements OnInit {
  users: User[] = [];
  editingUser: User | null = null;
  private router: Router;
  searchName: string = '';
  filteredNames: User[] = [];
  currentUser: User | null = null;
  confirmPassword: string = '';
  passwordMismatch: boolean = false;
  weakPassword: boolean = false;
  archiveNote: string = '';
  showErrorNote: boolean = false;

  constructor(private userService: UserService, router: Router) {
    this.router = router;
  }

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
    this.getCurrentUser();
  }

  getCurrentUser(): void {
    const user = localStorage.getItem('currentUser');
    if (user) {
      this.currentUser = JSON.parse(user);
    }
  }

  getUsers(): void {
    this.userService.getAllStudents().subscribe(
      (users) => {
        this.users = users;
        this.filterUsers();
      },
      (error) => {
        this.handleError(error);
      }
    );
  }

  selectUser(user: User): void {
    this.editingUser = { ...user };
    delete (this.editingUser as any).password;
    this.confirmPassword = '';
    this.passwordMismatch = false;
    this.weakPassword = false;
  }

  filterUsers(): void {
    this.filteredNames = this.users.filter(
      (user) =>
        user.name.toLowerCase().includes(this.searchName.toLowerCase()) ||
        user.email.toLowerCase().includes(this.searchName.toLowerCase())
    );
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

      //Only call this if password is valid
      this.userService.updatePassword(user).subscribe(
        (updatedUser) => {
          const index = this.users.findIndex((u) => u.id === updatedUser.id);
          if (index !== -1) {
            this.users[index] = updatedUser;
          }
          this.getUsers();
          this.editingUser = null;
          alert('Elev opdateret!');
        },
        (error) => {
          this.handleError(error);
        }
      );
    } else {
      // Only called when no password update is needed
      user.password = ''; // prevent sending null
      this.userService.update(user).subscribe(
        (updatedUser) => {
          const index = this.users.findIndex((u) => u.id === updatedUser.id);
          if (index !== -1) {
            this.users[index] = updatedUser;
          }
          this.getUsers();
          this.editingUser = null;
          alert('Elev opdateret!');
        },
        (error) => {
          this.handleError(error);
        }
      );
    }
  }

  resetPasswordValidation(): void {
    this.passwordMismatch = false;
    this.weakPassword = false;
  }

  onSave(): void {
    if (this.editingUser) {
      this.updateUser(this.editingUser);
    } else {
      alert('Kunne ikke opdatere elev!');
    }
  }

  confirmArchiveUser(): void {
    if (!this.archiveNote.trim()) {
      this.showErrorNote = true;
      return;
    }

    if (!this.editingUser?.id) {
      return;
    }

    this.userService.delete(this.editingUser.id, this.archiveNote).subscribe({
      next: () => {
        this.getUsers();
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

  closeUserDetailPanel(): void {
    this.editingUser = null;
  }

  navigateToCreateUser(): void {
    this.router.navigate(['/create-user']);
  }
}
