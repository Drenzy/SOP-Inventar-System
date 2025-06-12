import { Injectable, NgZone } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, map } from 'rxjs';
import { Router } from '@angular/router';
import { environment } from '../environments/environment';
import { Login } from '../models/login';
import { jwtDecode } from 'jwt-decode';

interface JwtPayload {
  exp: number;
  // ... other properties if needed
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private currentUserSubject: BehaviorSubject<Login | null>;
  public currentUser: Observable<Login | null>;

  constructor(
    private http: HttpClient,
    private router: Router,
    private ngZone: NgZone
  ) {
    let user: Login | null = null;
    if (typeof localStorage !== 'undefined') {
      user = JSON.parse(localStorage.getItem('currentUser') as string);
    }
    this.currentUserSubject = new BehaviorSubject<Login | null>(user);
    this.currentUser = this.currentUserSubject.asObservable();

    // On startup, if a user exists but the token has expired, log out.
    if (user && user.token && this.getTokenExpiration(user.token) <= Date.now()) {
      this.logout();
    }
  }

  public get currentUserValue(): Login | null {
    return this.currentUserSubject.value;
  }

  // Login method that saves the JWT token (with exp claim)
  login(email: string, password: string, role: string) {
    const authenticateUrl = `${environment.apiUrl}User/authenticate`;
    return this.http.post<any>(authenticateUrl, { email, password, role }).pipe(
      map((response) => {
        localStorage.setItem('currentUser', JSON.stringify(response));
        this.currentUserSubject.next(response);
        return response;
      })
    );
  }

  // Logout functionality
  logout() {
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
    this.router.navigate(['/login']);
  }

  // Decodes the token to get the expiration timestamp (converted to milliseconds)
  private getTokenExpiration(token: string): number {
    try {
      const decoded = jwtDecode<JwtPayload>(token);
      return decoded.exp * 1000;
    } catch (error) {
      return 0;
    }
  }

  // Returns the remaining time (in ms) until the token expires
  public getRemainingTokenTime(): number {
    const currentUser = this.currentUserValue;
    if (currentUser && currentUser.token) {
      const expirationTime = this.getTokenExpiration(currentUser.token);
      const remaining = expirationTime - Date.now();
      return remaining > 0 ? remaining : 0;
    }
    return 0;
  }

  // Extend token API call.
  public extendToken(): Observable<any> {
    const currentUser = this.currentUserValue;
    return this.http.post<any>(`${environment.apiUrl}User/extend-token`, { token: currentUser?.token }).pipe(
      map((response: any) => {
        // Ensure the property name (Token or token) matches what is returned by your API.
        const newToken = response.Token || response.token;
        if (currentUser && newToken) {
          // Merge all existing properties with the updated token.
          const updatedUser = { ...currentUser, token: newToken };
          localStorage.setItem('currentUser', JSON.stringify(updatedUser));
          this.currentUserSubject.next(updatedUser);
          console.log('[extendToken] Updated user object:', updatedUser);
        }
        return response;
      })
    );
  }
}