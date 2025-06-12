import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PartType } from '../models/partType';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class PartTypeService {

  //* The URL to the API endpoint for part types. \\
  private readonly apiUrl = environment.apiUrl + 'PartType';

  constructor(private http: HttpClient, private authService: AuthService) { }

  //* getting the current user from the  AuthService\\
  currentUser = this.authService.currentUserValue;

  //* Method for getting all part types. \\
  getAll(): Observable<PartType[]> {
    return this.http.get<PartType[]>(this.apiUrl, { headers: new HttpHeaders({'Authorization': 'Bearer ' + this.currentUser?.token})});
  }

  //* Method for getting a part type by ID. \\
  findById(partTypeId: number): Observable<PartType> {
    return this.http.get<PartType>(this.apiUrl + '/' + partTypeId, { headers: new HttpHeaders({'Authorization': 'Bearer ' + this.currentUser?.token})})
  }

  //* Method for creating a part type. \\
  create(partType: PartType): Observable<PartType> {
    return this.http.post<PartType>(this.apiUrl, partType, { headers: new HttpHeaders({'Authorization': 'Bearer ' + this.currentUser?.token})});
  }

  //* Method for deleting a part type by ID. \\
  update(partType: PartType): Observable<PartType> {
    return this.http.put<PartType>(this.apiUrl + '/' + partType.id, partType, { headers: new HttpHeaders({'Authorization': 'Bearer ' + this.currentUser?.token})});
  }
}
