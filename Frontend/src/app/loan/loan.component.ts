import { Component, OnInit } from '@angular/core';
import { NavbarComponent } from '../navbar/navbar.component';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { LoanService } from '../services/loan.service';
import { ItemService } from '../services/item.service';
import { UserService } from '../services/user.service';
import { Loan } from '../models/loan';
import { Item } from '../models/item';
import { User } from '../models/user';

@Component({
  selector: 'app-loan',
  standalone: true,
  imports: [NavbarComponent, FormsModule, CommonModule],
  templateUrl: './loan.component.html',
  styleUrl: './loan.component.css',
})
export class LoanComponent implements OnInit {
  currentUser: any;
  loans: Loan[] = [];
  items: Item[] = [];
  availableItems: Item[] = [];
  filteredAvailableItems: Item[] = [];
  filteredLoans: Loan[] = [];
  users: User[] = [];
  filteredUsers: User[] = [];
  newLoan: Loan = {
    id: 0,
    itemId: 0,
    userId: 0,
    loanDate: new Date(),
    returnDate: new Date(),
  };
  selectedLoan: Loan | null = null;
  isEditing: boolean = false;
  searchAvailableItemsTerm: string = '';
  searchLoansTerm: string = '';

  archiveNote: string = '';
  showErrorNote: boolean = false;



  constructor(
    private loanService: LoanService,
    private itemService: ItemService,
    private userService: UserService
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

  ngOnInit() {
    this.currentUser = JSON.parse(
      localStorage.getItem('currentUser') as string
    );
    this.getLoans();
    this.getAvailableItems();
    this.getUsers();
  }

  // Method to get all loans. \\
  getLoans() {
    this.loanService.getAll().subscribe(
      (loans) => {
        this.loans = loans;
        this.filterLoans();
      },
      (error) => {
        console.error('Error fetching loans', error);
        this.handleError(error);
      }
    );
  }

  // Method to get all available items. \\
  getAvailableItems() {
    this.itemService.getAll().subscribe(
      (items) => {
        this.items = items;
        this.filterAvailableItems();
      },
      (error) => {
        console.error('Error Available fetching items', error);
        this.handleError(error);
      }
    );
  }

  // Method to get all items. \\
  getItems() {
    this.itemService.getAll().subscribe(
      (items) => {
        this.items = items;
      },
      (error) => {
        console.error('Error fetching items', error);
        this.handleError(error);
      }
    );
  }

  // Method to get all users. \\
  getUsers() {
    this.userService.getAll().subscribe((users) => {
      this.users = users;
    });
  }

  // Method to filter available items. \\
  filterAvailableItems() {
    this.filteredAvailableItems = this.items.filter(
      (item) =>
        item.loan === null &&
        (item.serialNumber
          .toLowerCase()
          .includes(this.searchAvailableItemsTerm.toLowerCase()) ||
          item.itemGroup?.modelName
            .toLowerCase()
            .includes(this.searchAvailableItemsTerm.toLowerCase()))
    );
  }

  // Method to filter loans. \\
  filterLoans() {
    this.filteredLoans = this.loans.filter(
      (loan) =>
        loan.loanItem?.serialNumber
          .toLowerCase()
          .includes(this.searchLoansTerm.toLowerCase()) ||
        loan.loanUser?.email
          .toLowerCase()
          .includes(this.searchLoansTerm.toLowerCase())
    );
  }

  loanItem(item: Item) {
    this.newLoan.itemId = item.id;
  }

  // Method to create a loan. \\
  createLoan() {
    if (this.isEditing) {
      this.updateLoan();
    } else {
      // Check if the item is already loaned
      const existingLoan = this.loans.find(
        (loan) => loan.itemId === this.newLoan.itemId
      );
      if (existingLoan) {
        alert('This item is already loaned.');
        return;
      }

      this.loanService.create(this.newLoan).subscribe(
        (data) => {
          this.loans.push(data);
          this.resetForm();
          this.ngOnInit();
        },
        (error) => {
          console.error('Error creating loan', error);
          this.handleError(error);
        }
      );
    }
  }

  // Metod to update a loan. \\
  updateLoan() {
    // Check if the item is already loaned by another loan
    const existingLoan = this.loans.find(
      (loan) =>
        loan.itemId === this.newLoan.itemId && loan.id !== this.newLoan.id
    );
    if (existingLoan) {
      alert('This item is already loaned.');
      return;
    }

    this.loanService.update(this.newLoan).subscribe(
      (data) => {
        const index = this.loans.findIndex((loan) => loan.id === data.id);
        if (index !== -1) {
          this.loans[index] = data;
        }
        this.resetForm();
        this.ngOnInit();
      },
      (error) => {
        console.error('Error updating loan', error);
        this.handleError(error);
      }
    );
  }

  // Method to edit a loan. \\
  editLoan(loan: Loan) {
    this.isEditing = true;
    this.getItems();
    this.newLoan = { ...loan }; // Create a copy of the loan object to avoid modifying the original
  }


  cancelEdit() {
    this.resetForm();
  }

  // Method to reset the form. \\
  resetForm() {
    this.newLoan = {
      id: 0,
      itemId: 0,
      userId: 0,
      loanDate: new Date(),
      returnDate: new Date(),
    };
    this.isEditing = false;
    this.getAvailableItems(); // Fetch available items when resetting the form
  }

  // Add this new method to filter items for editing
  getAvailableItemsForEdit(): Item[] {
    if (!this.isEditing) {
      return this.filteredAvailableItems;
    }

    return this.items.filter(
      (item) =>
        item.loan === null || // Available items
        item.id === this.newLoan.itemId // Currently edited item
    );
  }

  confirmArchiveLoan(): void {
    if (!this.archiveNote.trim()) {
      this.showErrorNote = true;
      return;
    }

    if (!this.selectedLoan?.id) {
      console.log('No loan selected for deletion');
      return;
    }

    this.loanService.delete(this.selectedLoan.id, this.archiveNote).subscribe({
      next: () => {
        window.location.reload();
      },
      error: (error) => {
        this.handleError(error);
      },
    });
  }

  openArchiveModal(loan: Loan): void {
    this.selectedLoan = loan;
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
}
