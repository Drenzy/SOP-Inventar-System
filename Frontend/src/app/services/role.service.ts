import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Role } from '../models/role';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class RoleService {
  //* The URL to the API endpoint for roles. \\
  private readonly apiUrl = environment.apiUrl + 'Role';

  //* getting the current user from the  AuthService\\
  currentUser: any;

  constructor(private http: HttpClient, private authService: AuthService) {
    this.currentUser = this.authService.currentUserValue;
  }

  //* Method for getting all roles. \\
  getAll(): Observable<Role[]> {
    return this.http.get<Role[]>(this.apiUrl);
  }

  //* Method for getting a role by ID. \\
  findById(roleId: number): Observable<Role> {
    return this.http.get<Role>(this.apiUrl + '/' + roleId);
  }
}
