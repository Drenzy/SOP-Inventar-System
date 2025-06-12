import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../../services/auth.service';
import { ArchiveLoan } from '../archive-models/archive-loan';

@Injectable({
  providedIn: 'root',
})
export class ArchiveLoanService {
  //* The URL to the API endpoint for items. \\
  private readonly apiUrl = environment.apiUrl + 'Archive_Loan';

  //* getting the current user from the  AuthService\\
  currentUser: any;

  constructor(private http: HttpClient, private authService: AuthService) {
    this.currentUser = this.authService.currentUserValue;
  }

  getAll(): Observable<ArchiveLoan[]> {
    return this.http.get<ArchiveLoan[]>(this.apiUrl);
  }

  //* Method for deleting a item by ID. \\
  delete(Id: number): Observable<ArchiveLoan> {
    return this.http.delete<ArchiveLoan>(this.apiUrl + '/' + Id);
  }

  //* Method for restoring a loan by ID. \\
  restore(Id: number): Observable<ArchiveLoan> {
    return this.http.post<ArchiveLoan>(this.apiUrl + '/RestoreById/' + Id, null);
  }
}
