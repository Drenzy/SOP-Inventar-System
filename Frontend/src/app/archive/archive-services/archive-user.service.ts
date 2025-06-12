import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../../services/auth.service';
import { ArchiveUser } from '../archive-models/archive-user';

@Injectable({
  providedIn: 'root',
})
export class ArchiveUserService {
  //* The URL to the API endpoint for items. \\
  private readonly apiUrl = environment.apiUrl + 'Archive_User';

  //* getting the current user from the  AuthService\\
  currentUser: any;

  constructor(private http: HttpClient, private authService: AuthService) {
    this.currentUser = this.authService.currentUserValue;
  }

  getAll(): Observable<ArchiveUser[]> {
    return this.http.get<ArchiveUser[]>(this.apiUrl);
  }

  //* Method for deleting a item by ID. \\
  delete(Id: number): Observable<ArchiveUser> {
    return this.http.delete<ArchiveUser>(this.apiUrl + '/' + Id);
  }

  //* Method for restoring an user by ID. \\
  restore(Id: number): Observable<ArchiveUser> {
    return this.http.post<ArchiveUser>(this.apiUrl + '/RestoreById/' + Id, null);
  }
}
