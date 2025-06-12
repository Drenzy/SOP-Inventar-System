import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  standalone: true,
  imports: [FormsModule, CommonModule],
})
export class LoginComponent implements OnInit {
  email: string = '';
  password: string = '';
  role: string = '';

  constructor(private router: Router, private authService: AuthService) { }


  private handleError(error: any): void {
    switch (error.status) {
      case 0:
        alert('Ingen forbindelse til server. Prøv igen senere.');
        break;
      case 400:
        alert('Email eller kode er forkert, prøv igen');
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
    if (this.authService.currentUserValue) {
      this.router.navigate(['/home']);
    }
  }

  login() {
    this.authService.login(this.email, this.password, this.role).subscribe({
      next: () => {
        this.router.navigate(['/home']);
      },
      error: (error) => {
        this.handleError(error);
      }
    });
  }
}
