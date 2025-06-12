import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Status } from '../models/status';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class StatusService {
  //* The URL to the API endpoint for statuses. \\
  private readonly apiUrl = environment.apiUrl + 'Status';

  //* getting the current user from the  AuthService\\
  currentUser: any;

  constructor(private http: HttpClient, private authService: AuthService) {
    this.currentUser = this.authService.currentUserValue;
  }

  //* Method for getting all statuses. \\
  getAll(): Observable<Status[]> {
    return this.http.get<Status[]>(this.apiUrl);
  }

  //* Method for getting a status by ID. \\
  findById(statusId: number): Observable<Status> {
    return this.http.get<Status>(this.apiUrl + '/' + statusId);
  }

  //* Method for creating a status. \\
  create(status: Status): Observable<Status> {
    return this.http.post<Status>(this.apiUrl, status);
  }
}
