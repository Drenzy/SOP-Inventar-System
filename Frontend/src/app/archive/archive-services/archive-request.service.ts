import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../../services/auth.service';
import { ArchiveRequest } from '../archive-models/archive-request';

@Injectable({
  providedIn: 'root',
})
export class ArchiveRequestService {
  //* The URL to the API endpoint for items. \\
  private readonly apiUrl = environment.apiUrl + 'Archive_Request';

  //* getting the current user from the  AuthService\\
  currentUser: any;

  constructor(private http: HttpClient, private authService: AuthService) {
    this.currentUser = this.authService.currentUserValue;
  }

  getAll(): Observable<ArchiveRequest[]> {
    return this.http.get<ArchiveRequest[]>(this.apiUrl);
  }

  //* Method for deleting a item by ID. \\
  delete(Id: number): Observable<ArchiveRequest> {
    return this.http.delete<ArchiveRequest>(this.apiUrl + '/' + Id);
  }

  //* Method for restoring a request by ID. \\
  restore(Id: number): Observable<ArchiveRequest> {
    return this.http.post<ArchiveRequest>(this.apiUrl + '/RestoreById/' + Id, null);
  }
}
