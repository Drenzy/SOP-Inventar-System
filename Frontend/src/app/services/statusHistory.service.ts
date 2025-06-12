import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { StatusHistory } from '../models/statusHistory';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class StatusHistoryService {
  //* The URL to the API endpoint for status histories. \\
  private readonly apiUrl = environment.apiUrl + 'StatusHistory';

  //* getting the current user from the  AuthService\\
  currentUser: any;

  constructor(private http: HttpClient, private authService: AuthService) {
    this.currentUser = this.authService.currentUserValue;
  }

  //* Method for getting all status histories. \\
  getAll(): Observable<StatusHistory[]> {
    return this.http.get<StatusHistory[]>(this.apiUrl);
  }

  //* Method for getting a status history by ID. \\
  findById(statusHistoryId: number): Observable<StatusHistory> {
    return this.http.get<StatusHistory>(this.apiUrl + '/'+ statusHistoryId);
  }

  //* Method for creating a status history. \\
  create(statusHistory: StatusHistory): Observable<StatusHistory> {
    return this.http.post<StatusHistory>(this.apiUrl, statusHistory);
  }

  //* Method for updating a status history. \\
  update(statusHistory: StatusHistory): Observable<StatusHistory> {
    return this.http.put<StatusHistory>(this.apiUrl + '/' +statusHistory.id, statusHistory);
  }
}
