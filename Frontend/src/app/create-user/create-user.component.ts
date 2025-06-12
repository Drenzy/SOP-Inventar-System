import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { NavbarComponent } from '../navbar/navbar.component';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-create-user',
  standalone: true,
  imports: [CommonModule, FormsModule, NavbarComponent],
  templateUrl: './create-user.component.html',
  styleUrls: ['./create-user.component.css'],
})
export class CreateUserComponent implements OnInit {
  user: any = {};
  confirmPassword: string = '';
  currentUser: any = null;

  passwordMismatch = false;
  weakPassword = false;

  constructor(private userService: UserService, private router: Router) { }

  ngOnInit(): void {
    const userFromStorage = localStorage.getItem('currentUser');
    if (userFromStorage) {
      this.currentUser = JSON.parse(userFromStorage);
    }
  }

  validatePassword(): void {
    const password = this.user.password || '';
    this.passwordMismatch = password !== this.confirmPassword;

    const isValid = [
      /[A-Z]/.test(password),
      /[a-z]/.test(password),
      /[0-9]/.test(password),
      /[!@#$%^&*(),-.?":{}|<>]/.test(password),
      password.length >= 15
    ].every(Boolean);

    this.weakPassword = !isValid;
  }

  createUser(form: NgForm): void {
    if (this.passwordMismatch || this.weakPassword || !form.valid) {
      return;
    }

    this.userService.create(this.user).subscribe({
      next: (response) => {
        console.log('User created:', response);
        form.resetForm();
        this.navigateAfterCreate();
      },
      error: (error) => {
        this.handleError(error);
      }
    });
  }

  onRoleChange(event: Event): void {
    const select = event.target as HTMLSelectElement;
    this.user.roleId = +select.value;
  }

  private navigateAfterCreate(): void {
    if (!this.currentUser?.role?.id) {
      this.router.navigate(['/']);
      return;
    }

    switch (this.currentUser.role.id) {
      case 1:
        this.router.navigate(['/users']);
        break;
      case 2:
      case 4:
        this.router.navigate(['/students']);
        break;
      default:
        this.router.navigate(['/']);
    }
  }

  private handleError(error: any): void {
    let message = 'En ukendt fejl opstod. Prøv igen senere.';

    if (error.status === 0) {
      message = 'Ingen forbindelse til server. Prøv igen senere.';
    } else if (error.status === 400) {
      message = 'Fejl i forespørgsel til serveren. Prøv igen senere.';
    } else if (error.status === 500) {
      message = 'Fejl på serveren. Prøv igen senere.';
    }

    alert(message);
  }
}
