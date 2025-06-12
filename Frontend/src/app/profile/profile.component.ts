import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { UserService } from '../services/user.service';
import { User } from '../models/user';
import { Loan } from '../models/loan';
import { Item } from '../models/item';
import { NavbarComponent } from '../navbar/navbar.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Login } from '../models/login';
import { Request } from '../models/request';
import { RequestService } from '../services/request.service';

@Component({
  selector: 'app-profile',
  standalone: true,
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
  imports: [NavbarComponent, CommonModule, FormsModule],
})
export class ProfileComponent implements OnInit {
  users: User[] = [];
  loans: Loan[] = [];
  instuctors: User[] = [];
  confirmPassword: string = '';
  passwordMismatch: boolean = false;

  weakPassword: boolean = false;

  item: Item = {
    id: 0,
    roomId: 0,
    itemGroupId: 0,
    serialNumber: '',
    itemGroup: {
      id: 0,
      itemTypeId: 0,
      modelName: '',
      quantity: 0,
      price: 0,
      manufacturer: '',
      warrantyPeriod: '',
      itemType: {
        id: 0,
        typeName: '',
      },
    },
  };

  loan: Loan = {
    id: 0,
    loanDate: new Date('0000-00-00'),
    returnDate: new Date('0000-00-00'),
    itemId: 0,
    userId: 0,
  };

  user: User = {
    id: 0,
    name: '',
    email: '',
    password: '',
    twoFactorAuthentication: false,
    roleId: 0,
    role: {
      id: 0,
      name: '',
      description: '',
    },
    userLoans: [],
  };

  localCurrentUserLogin: Login = {
    id: 0,
    email: '',
    password: '',
    twoFactorAuthentication: false,
    token: '',
    role: {
      id: 0,
      name: '',
      description: '',
    },
  };

  instuctor: User = {
    id: 0,
    name: '',
    email: '',
    password: '',
    twoFactorAuthentication: false,
    roleId: 0,
    role: {
      id: 0,
      name: '',
      description: '',
    },
  };

  request: Request = {
    id: 0,
    userId: 0,
    recipientEmail: '',
    item: '',
    message: '',
    date: new Date(),
    status: '',
  };

  showRequestModal: boolean = false;
  remainingTime: number = 0;

  constructor(
    private router: Router,
    private authService: AuthService,
    private userService: UserService,
    private requestService: RequestService
  ) { }

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

  async ngOnInit(): Promise<void> {
    this.remainingTime = this.authService.getRemainingTokenTime();
    console.log('Remaining token time (ms):', this.remainingTime);
    await this.getUser();
    this.getInstuctor();
  }

  // Method to get the user. \\
  async getUser(): Promise<void> {
    this.authService.currentUser.subscribe((x) => {
      if (x && x.id !== 0) {
        this.localCurrentUserLogin = x;
        this.userService.findById(this.localCurrentUserLogin.id).subscribe(
          (data) => {
            // Ensure that role is properly initialized if missing
            this.user = data;
            this.selectUser(data);
          },
          (error) => {
            console.error('Error fetching users', error);
            this.handleError(error);
          }
        );
      }
    });
  }

  // Method to get the instructor.
  getInstuctor(): void {
    this.userService.getUseresByRoleId(4).subscribe(
      (drift) => {
        this.instuctors = drift; // Add the first set of users
        this.userService.getUseresByRoleId(2).subscribe(
          (instructor) => {
            this.instuctors = [...this.instuctors, ...instructor]; // Append the second set of users
          },
          (error) => {
            console.error('Error fetching users', error);
            this.handleError(error);
          }
        );
      },
      (error) => {
        console.error('Error fetching users', error);
        this.handleError(error);
      }
    );
  }

  // Method to select the user. \\
  selectUser(user: User): void {
    this.user = { ...user };
    this.user.password = ''; // Clear the password field
    this.confirmPassword = ''; // Clear the confirm password field
    this.checkPasswords(); // Check passwords initially
  }

  // Check if passwords match and meet strength requirements
  checkPasswords() {
    const password = this.user.password;
    const confirmPassword = this.confirmPassword;

    // Password match check
    this.passwordMismatch = password !== confirmPassword;

    // Password strength check
    const hasUpperCase = /[A-Z]/.test(password);
    const hasLowerCase = /[a-z]/.test(password);
    const hasNumber = /[0-9]/.test(password);
    const hasSpecialChar = /[!@#$%^&*(),-.?":{}|<>]/.test(password);
    const hasMinLength = password?.length >= 15;

    this.weakPassword = !(
      hasUpperCase &&
      hasLowerCase &&
      hasNumber &&
      hasSpecialChar &&
      hasMinLength
    );
  }


  // Method to save the changes. \\
  saveChanges(): void {
    if (!this.user) {
      console.log('No user to update');
      return;
    }

    this.updateUser(this.user);
  }


  // Method to update the user. \\
  updateUser(user: User): void {
    user.roleId = Number(user.roleId);

    if (user.password) {
      if (this.passwordMismatch) {
        console.log('Passwords do not match');
        return;
      }

      if (this.weakPassword) {
        alert('Fejl: Adgangskoden er for svag. Den skal være mindst 12 tegn og indeholde store og små bogstaver, tal og specialtegn.');
        return;
      }

      this.userService.updatePassword(user).subscribe(
        (updatedUser) => {
          const index = this.users.findIndex((u) => u.id === updatedUser.id);
          if (index !== -1) {
            this.users[index] = updatedUser;
          }
          this.getUser();
        },
        (error) => {
          console.error('Error updating user with password', error);
          this.handleError(error);
        }
      );
    } else {

      user.password = ''; // Clear the password field if not updating it
      this.userService.update(user).subscribe(
        (updatedUser) => {
          const index = this.users.findIndex((u) => u.id === updatedUser.id);
          if (index !== -1) {
            this.users[index] = updatedUser;
          }
          this.getUser();
        },
        (error) => {
          console.error('Error updating user without password', error);
          this.handleError(error);
        }
      );
    }
  }


  // Method for navigation to item details. \\
  navigateToItemDetails(itemId: number | undefined): void {
    if (itemId !== undefined) {
      if (this.user.roleId === 3) {
        return;
      } else {
        this.router.navigate(['/itemDetails/', itemId]);
      }
    }
  }

  // Method for opening the request modal. \\
  openRequestModal(): void {
    console.log(this.instuctors);
    this.showRequestModal = true;
  }

  // Method for closing the request modal. \\
  closeEditModal(): void {
    this.request = {
      id: 0,
      userId: 0,
      recipientEmail: '',
      item: '',
      message: '',
      date: new Date(),
      status: '',
    };
    this.showRequestModal = false;
  }

  async bindRequest(): Promise<void> {
    this.request.userId = this.user.id;
    this.request.date = new Date();
    this.request.status = 'Sent';
    console.log('Adding request:', this.request);
  }

  // Method for creating a request. \\
  async createRequest(): Promise<void> {
    await this.bindRequest();
    this.requestService.create(this.request).subscribe(
      (response) => {
        console.log('Request created successfully:', response);
        console.log(this.request);
        this.closeEditModal();
      },
      (error) => {
        console.error('Error adding request', error);
        this.handleError(error);
      }
    );
  }
}
