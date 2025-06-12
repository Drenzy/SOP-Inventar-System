import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Computer_ComputerPart } from '../models/computer_ComputerPart';
import { AuthService } from './auth.service';


@Injectable({
  providedIn: 'root'
})
export class Computer_ComputerPartService {

  //* The URL to the API endpoint for computer_ComputerParts. \\
  private readonly apiUrl = environment.apiUrl + 'Computer_ComputerPart';

  constructor(private http: HttpClient, private authService: AuthService) { }

  //* getting the current user from the  AuthService\\
  currentUser = this.authService.currentUserValue;

  //* Method for getting all computer_ComputerParts. \\
  getAll(): Observable<Computer_ComputerPart[]> {
    return this.http.get<Computer_ComputerPart[]>(this.apiUrl, { headers: new HttpHeaders({'Authorization': 'Bearer ' + this.currentUser?.token})});
  }

  //* Method for getting a computer_ComputerPart by ID. \\
  findById(Id: number): Observable<Computer_ComputerPart> {
    return this.http.get<Computer_ComputerPart>(this.apiUrl + '/' + Id, { headers: new HttpHeaders({'Authorization': 'Bearer ' + this.currentUser?.token})})
  }

  //* Method for creating a computer_ComputerPart. \\
  create(Computer_ComputerPart: Computer_ComputerPart): Observable<Computer_ComputerPart> {
    return this.http.post<Computer_ComputerPart>(this.apiUrl, Computer_ComputerPart, { headers: new HttpHeaders({'Authorization': 'Bearer ' + this.currentUser?.token})});
  }

  //* Method for deleting a computer_ComputerPart by ID.
  delete(Id: number): Observable<Computer_ComputerPart> {
    return this.http.delete<Computer_ComputerPart>(this.apiUrl + '/' + Id, { headers: new HttpHeaders({'Authorization': 'Bearer ' + this.currentUser?.token})});
  }

  //* Method for updating a computer_ComputerPart. \\
  update(Computer_ComputerPart: Computer_ComputerPart): Observable<Computer_ComputerPart> {
    return this.http.put<Computer_ComputerPart>(this.apiUrl + '/' + Computer_ComputerPart.id, Computer_ComputerPart, { headers: new HttpHeaders({'Authorization': 'Bearer ' + this.currentUser?.token})});
  }
}
