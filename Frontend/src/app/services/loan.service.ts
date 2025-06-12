import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Loan } from '../models/loan';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class LoanService {
  private readonly apiUrl = environment.apiUrl + 'Loan';

  //* getting the current user from the  AuthService\\
  currentUser: any;

  constructor(private http: HttpClient, private authService: AuthService) {
    this.currentUser = this.authService.currentUserValue;
  }

  //* Method for getting all loans. \\
  getAll(): Observable<Loan[]> {
    return this.http.get<Loan[]>(this.apiUrl);
  }

  //* Method for getting a loan by ID. \\
  findById(loanId: number): Observable<Loan> {
    return this.http.get<Loan>(this.apiUrl + '/' + loanId);
  }

  //* Method for creating a loan. \\
  create(loan: Loan): Observable<Loan> {
    return this.http.post<Loan>(this.apiUrl, loan);
  }

  //* Method for deleting a loan by ID. \\
  delete(loanId: number, note: string): Observable<Loan> {
    return this.http.delete<Loan>(this.apiUrl + '/' + loanId, {
      body: { archiveNote: note }
    });
  }

  //* Method for updating a loan. \\
  update(loan: Loan): Observable<Loan> {
    return this.http.put<Loan>(this.apiUrl + '/' + loan.id, loan);
  }
}
