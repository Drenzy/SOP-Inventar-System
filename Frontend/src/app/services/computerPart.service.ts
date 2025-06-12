import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ComputerPart } from '../models/computerPart';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class ComputerPartService {

  //* The URL to the API endpoint for computer parts. \\
  private readonly apiUrl = environment.apiUrl + 'computerPart';

  constructor(private http: HttpClient, private authService: AuthService) { }

  //* getting the current user from the  AuthService\\
  currentUser = this.authService.currentUserValue;

  //* Method for getting all computer parts. \\
  getAll(): Observable<ComputerPart[]> {
    return this.http.get<ComputerPart[]>(this.apiUrl, { headers: new HttpHeaders({'Authorization': 'Bearer ' + this.currentUser?.token})});
  }

  //* Method for getting a computer part by ID. \\
  findById(Id: number): Observable<ComputerPart> {
    return this.http.get<ComputerPart>(this.apiUrl + '/' + Id, { headers: new HttpHeaders({'Authorization': 'Bearer ' + this.currentUser?.token})})
  }

  //* Method for creating a computer part. \\
  create(computerPart: ComputerPart): Observable<ComputerPart> {
    return this.http.post<ComputerPart>(this.apiUrl, computerPart, { headers: new HttpHeaders({'Authorization': 'Bearer ' + this.currentUser?.token})});
  }

  //* Method for deleting a computer part by ID. \\
  delete(Id: number): Observable<ComputerPart> {
    return this.http.delete<ComputerPart>(this.apiUrl + '/' + Id, { headers: new HttpHeaders({'Authorization': 'Bearer ' + this.currentUser?.token})});
  }

  //* Method for updating a computer part. \\
  update(computerPart: ComputerPart): Observable<ComputerPart> {
    return this.http.put<ComputerPart>(this.apiUrl + '/' + computerPart.id, computerPart, { headers: new HttpHeaders({'Authorization': 'Bearer ' + this.currentUser?.token})});
  }
}
