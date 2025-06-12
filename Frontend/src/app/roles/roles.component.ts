import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NavbarComponent } from '../navbar/navbar.component';
import { Role } from '../models/role';
import { RoleService } from '../services/role.service';

@Component({
  selector: 'app-roles',
  standalone: true,
  imports: [CommonModule, FormsModule, NavbarComponent],
  templateUrl: './roles.component.html',
  styleUrl: './roles.component.css',
})
export class RolesComponent implements OnInit {
  roles: Role[] = [];
  newRole: Role = {
    id: 0,
    name: '',
    description: '',
  };
  selectedRole: Role | null = null;
  searchRole: string = '';
  filteredRoles: Role[] = [];

  constructor(private roleService: RoleService) { }

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
    this.getRoles();
  }

  // Fetch all roles. \\
  getRoles(): void {
    this.roleService.getAll().subscribe(
      (roles) => {
        this.roles = roles;
        this.filteredRoles = this.roles;
      },
      (error) => {
        console.error('Error fetching roles', error);
        this.handleError(error);
      }
    );
  }

  // Method for filtering roles. \\
  filterRoles(): void {
    this.filteredRoles = this.roles.filter(
      (role) =>
        role.name.toLowerCase().includes(this.searchRole.toLowerCase()) ||
        role.description.toLowerCase().includes(this.searchRole.toLowerCase())
    );
  }
}
