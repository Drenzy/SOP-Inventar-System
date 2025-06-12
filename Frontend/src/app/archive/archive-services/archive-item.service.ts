import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../../services/auth.service';
import { ArchiveItem } from '../archive-models/archive-item';

@Injectable({
  providedIn: 'root',
})
export class ArchiveItemService {
  //* The URL to the API endpoint for items. \\
  private readonly apiUrl = environment.apiUrl + 'Archive_Item';

  //* getting the current user from the  AuthService\\
  currentUser: any;

  constructor(private http: HttpClient, private authService: AuthService) {
    this.currentUser = this.authService.currentUserValue;
  }

  getAll(): Observable<ArchiveItem[]> {
    return this.http.get<ArchiveItem[]>(this.apiUrl);
  }

  //* Method for deleting a item by ID. \\
  delete(Id: number): Observable<ArchiveItem> {
    return this.http.delete<ArchiveItem>(this.apiUrl + '/' + Id);
  }

  //* Method for restoring a item by ID. \\
  restore(Id: number): Observable<ArchiveItem> {
    return this.http.post<ArchiveItem>(this.apiUrl + '/RestoreById/' + Id, null);
  }
}
