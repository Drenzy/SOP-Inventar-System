import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../../services/auth.service';
import { ArchiveItemGroup } from '../archive-models/archive-itemgroup';

@Injectable({
  providedIn: 'root',
})
export class ArchiveItemGroupService {
  //* The URL to the API endpoint for items. \\
  private readonly apiUrl = environment.apiUrl + 'Archive_ItemGroup';

  //* getting the current user from the  AuthService\\
  currentUser: any;

  constructor(private http: HttpClient, private authService: AuthService) {
    this.currentUser = this.authService.currentUserValue;
  }

  getAll(): Observable<ArchiveItemGroup[]> {
    return this.http.get<ArchiveItemGroup[]>(this.apiUrl);
  }

  //* Method for deleting a item by ID. \\
  delete(Id: number): Observable<ArchiveItemGroup> {
    return this.http.delete<ArchiveItemGroup>(this.apiUrl + '/' + Id);
  }

  //* Method for restoring a item by ID. \\
  restore(Id: number): Observable<ArchiveItemGroup> {
    return this.http.post<ArchiveItemGroup>(this.apiUrl + '/RestoreById/' + Id, null);
  }
}
