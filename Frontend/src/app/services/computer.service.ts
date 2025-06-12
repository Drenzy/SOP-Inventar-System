import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Computer } from '../models/computer';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class ComputerService {

  //* The URL to the API endpoint for computers. \\
  private readonly apiUrl = environment.apiUrl + 'computer';

  constructor(private http: HttpClient, private authService: AuthService) { }

  //* getting the current user from the  AuthService\\
  currentUser = this.authService.currentUserValue;

  //* Method for getting all computers. \\
  getAll(): Observable<Computer[]> {
    return this.http.get<Computer[]>(this.apiUrl, { headers: new HttpHeaders({'Authorization': 'Bearer ' + this.currentUser?.token})});
  }

  //* Method for getting a computer by ID. \\
  findById(Id: number): Observable<Computer> {
    return this.http.get<Computer>(this.apiUrl + '/' + Id, { headers: new HttpHeaders({'Authorization': 'Bearer ' + this.currentUser?.token})})
  }

  //* Method for creating a computer. \\
  create(computer: Computer): Observable<Computer> {
    return this.http.post<Computer>(this.apiUrl, computer, { headers: new HttpHeaders({'Authorization': 'Bearer ' + this.currentUser?.token})});
  }

  //* Method for deleting a computer by ID. \\
  delete(Id: number): Observable<Computer> {
    return this.http.delete<Computer>(this.apiUrl + '/' + Id, { headers: new HttpHeaders({'Authorization': 'Bearer ' + this.currentUser?.token})});
  }

  //* Method for deleting a computer and its associated computer parts by ID. \\
  deleteComputerAndItem(Id: number): Observable<Computer> {
    return this.http.delete<Computer>(this.apiUrl + '/deleteComputerAndItem/' + Id, { headers: new HttpHeaders({'Authorization': 'Bearer ' + this.currentUser?.token})});
  }

  //* Method for updating a computer. \\
  update(computer: Computer): Observable<Computer> {
    return this.http.put<Computer>(this.apiUrl + '/' + computer.id, computer, { headers: new HttpHeaders({'Authorization': 'Bearer ' + this.currentUser?.token})});
  }
}
