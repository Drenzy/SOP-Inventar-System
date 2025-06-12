import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { CommonModule } from '@angular/common';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
  standalone: true,
  imports: [CommonModule, RouterModule],
})
export class NavbarComponent implements OnInit, OnDestroy {
  currentUser: any;
  showExtendModal: boolean = false;
  private sessionTimer: any;
  private authSub!: Subscription;

  constructor(public authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    this.currentUser = JSON.parse(localStorage.getItem('currentUser') as string);
    this.setSessionTimer();

    // Reset timer whenever the currentUser value changes (e.g., after extending token).
    this.authSub = this.authService.currentUser.subscribe(() => {
      this.clearSessionTimer();
      this.setSessionTimer();
    });
  }

  // Set a timer based on the remaining token validity time.
  private setSessionTimer(): void {
    const remaining = this.authService.getRemainingTokenTime();
    if (remaining > 10000) {
      this.sessionTimer = setTimeout(() => {
        this.showExtendModal = true;
      }, remaining);
    } else {
      this.sessionTimer = setTimeout(() => {
        this.showExtendModal = true;
      }, 10000);
    }
  }

  private clearSessionTimer(): void {
    if (this.sessionTimer) {
      clearTimeout(this.sessionTimer);
      this.sessionTimer = null;
    }
  }

  // Called when the user confirms to extend the session.
  extendSession(): void {
    console.log('[extendSession] Extending session...');
    this.authService.extendToken().subscribe({
      next: (response: any) => {
        // alert('Session extended successfully!');
        this.showExtendModal = false; // explicitly close the modal on success
        this.setSessionTimer();
        // Update currentUser from local storage.
        this.currentUser = JSON.parse(localStorage.getItem('currentUser') as string);
      },
      error: (error: any) => {
        alert('Fejl ved at fornye din session: ' + error.error);
        this.showExtendModal = false; // explicitly close the modal on error
        this.authService.logout();
        this.router.navigate(['/login']);
      }
    });
  }

  // Called when the user clicks logout manually.
  logout(): void {
    if (confirm('Vil du gerne logge ud?')) {
      this.authService.logout();
      this.router.navigate(['/login']);
    }
  }

  ngOnDestroy(): void {
    this.clearSessionTimer();
    if (this.authSub) {
      this.authSub.unsubscribe();
    }
  }
}