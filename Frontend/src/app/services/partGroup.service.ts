import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PartGroup } from '../models/partGroup';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class PartGroupService {

  //* The URL to the API endpoint for part groups. \\
  private readonly apiUrl = environment.apiUrl + 'PartGroup';

  constructor(private http: HttpClient, private authService: AuthService) { }

  //* getting the current user from the  AuthService\\
  currentUser = this.authService.currentUserValue;

  //* Method for getting all part groups. \\
  getAll(): Observable<PartGroup[]> {
    return this.http.get<PartGroup[]>(this.apiUrl, { headers: new HttpHeaders({'Authorization': 'Bearer ' + this.currentUser?.token})});
  }

  //* Method for getting a part group by ID. \\
  findById(partGroupId: number): Observable<PartGroup> {
    return this.http.get<PartGroup>(this.apiUrl + '/' + partGroupId, { headers: new HttpHeaders({'Authorization': 'Bearer ' + this.currentUser?.token})})
  }

  //* Method for creating a part group. \\
  create(partGroup: PartGroup): Observable<PartGroup> {
    return this.http.post<PartGroup>(this.apiUrl, partGroup, { headers: new HttpHeaders({'Authorization': 'Bearer ' + this.currentUser?.token})});
  }

  //* Method for deleting a part group by ID. \\
  update(partGroup: PartGroup): Observable<PartGroup> {
    return this.http.put<PartGroup>(this.apiUrl + '/' + partGroup.id, partGroup, { headers: new HttpHeaders({'Authorization': 'Bearer ' + this.currentUser?.token})});
  }
}
