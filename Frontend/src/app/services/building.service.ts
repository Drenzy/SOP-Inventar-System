import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Building } from '../models/building';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class BuildingService {
  //* The URL to the API endpoint for buildings. \\
  private readonly apiUrl = environment.apiUrl + 'building';

  //* getting the current user from the  AuthService\\
  currentUser: any;

  constructor(private http: HttpClient, private authService: AuthService) {
    this.currentUser = this.authService.currentUserValue;
  }
  //* Method for getting all buildings. \\
  getAll(): Observable<Building[]> {
    return this.http.get<Building[]>(this.apiUrl);
  }

  //* Method for getting a building by ID. \\
  findById(Id: number): Observable<Building> {
    return this.http.get<Building>(this.apiUrl + '/' + Id);
  }

  //* Method for creating a building. \\
  create(building: Building): Observable<Building> {
    return this.http.post<Building>(this.apiUrl, building);
  }

  //* Method for deleting a building by ID. \\
  delete(Id: number): Observable<Building> {
    return this.http.delete<Building>(this.apiUrl + '/' + Id);
  }

  //* Method for updating a building. \\
  update(building: Building): Observable<Building> {
    return this.http.put<Building>(this.apiUrl + '/' + building.id, building);
  }
}
