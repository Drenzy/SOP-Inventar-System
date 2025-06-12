import { AuthService } from './auth.service';
import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  //* The URL to the API endpoint for users. \\
  private readonly apiUrl = environment.apiUrl + 'User';

  //* getting the current user from the  AuthService\\
  currentUser: any;

  constructor(private http: HttpClient, private authService: AuthService) {
    this.currentUser = this.authService.currentUserValue;
  }
  //* Method for getting all users. \\
  getAll(): Observable<User[]> {
    return this.http.get<User[]>(this.apiUrl);
  }

  //* Method for getting all students. \\
  getAllStudents(): Observable<User[]> {
    return this.http.get<User[]>(this.apiUrl + '/GetAllStudents');
  }

  //* Method for getting all students. \\
  getUseresByRoleId(roleId: number): Observable<User[]> {
    return this.http.get<User[]>(this.apiUrl + '/GetUsersByRoleId/' + roleId);
  }


  //* Find a user by ID. \\
  findById(userId: number): Observable<User> {
    return this.http.get<User>(this.apiUrl + '/' + userId);
  }

  //* Method for creating a user. \\
  create(user: User): Observable<User> {
    return this.http.post<User>(this.apiUrl, user);
  }

  //* Method for deleting a user. \\
  delete(userId: number, note: string): Observable<User> {
    return this.http.delete<User>(this.apiUrl + '/ArchiveById/' + userId, {
      body: { archiveNote: note },
    });
  }

  //* Method for updating a user. \\
  update(user: User): Observable<User> {
    return this.http.put<User>(this.apiUrl + '/' + user.id, user);
  }

  //* Method for only updating an users password. \\
  updatePassword(user: User): Observable<User> {
    return this.http.put<User>(
      this.apiUrl + '/updatePassword/' + user.id,
      {
        name: user.name,
        email: user.email,
        password: user.password,
        roleId: user.roleId,
        twoFactorAuthentication: user.twoFactorAuthentication,
      },
    );
  }
}
