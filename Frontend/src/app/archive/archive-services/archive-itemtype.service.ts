import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../../services/auth.service';
import { ArchiveItemType } from '../archive-models/archive-itemtype';

@Injectable({
  providedIn: 'root',
})
export class ArchiveItemTypeService {
  //* The URL to the API endpoint for items. \\
  private readonly apiUrl = environment.apiUrl + 'Archive_ItemType';

  //* getting the current user from the  AuthService\\
  currentUser: any;

  constructor(private http: HttpClient, private authService: AuthService) {
    this.currentUser = this.authService.currentUserValue;
  }

  getAll(): Observable<ArchiveItemType[]> {
    return this.http.get<ArchiveItemType[]>(this.apiUrl);
  }

  //* Method for deleting a item by ID. \\
  delete(Id: number): Observable<ArchiveItemType> {
    return this.http.delete<ArchiveItemType>(this.apiUrl + '/' + Id);
  }
  //* Method for restoring a item by ID. \\
  restore(Id: number): Observable<ArchiveItemType> {
    return this.http.post<ArchiveItemType>(this.apiUrl + '/RestoreById/' + Id, null);
  }
}
