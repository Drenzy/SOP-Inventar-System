import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Role } from '../models/role';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  standalone: true,
  imports: [CommonModule]
})
export class HomeComponent implements OnInit {

  roles: Role[] = [];

  constructor(private router: Router, private authService: AuthService) { }

  ngOnInit(): void {

    const currentUser = this.authService.currentUserValue;
    if (currentUser && currentUser.role) {
      // If user is admin, instruktør or drift, redirect to inventar
      if (currentUser.role.id === 1 || currentUser.role.id === 4) {
        this.router.navigate(['/inventory']);
      }
      else if (currentUser.role.id === 2) {
        this.router.navigate(['/students']);
      }
      // If user is student, redirect to profile 
      else {
        this.router.navigate(['/profile']);
      }
    }
    // If user is not logged in, redirect to login
    else {
      this.router.navigate(['/login']);
    }
  }
}