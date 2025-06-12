import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root',
})
export class RoleGuard implements CanActivate {
  constructor(private router: Router, private authService: AuthService) { }

  canActivate(route: ActivatedRouteSnapshot): boolean {
    const currentUser = this.authService.currentUserValue;
    const allowedRoles = route.data['roles'] as Array<number>;

    // Check if there is a user, the token has not expired, and the user's role is allowed.
    if (
      currentUser &&
      this.authService.getRemainingTokenTime() > 0 &&
      allowedRoles.includes(currentUser.role.id)
    ) {
      return true;
    } else {
      this.authService.logout();
      this.router.navigate(['/login']);
      return false;
    }
  }
}
