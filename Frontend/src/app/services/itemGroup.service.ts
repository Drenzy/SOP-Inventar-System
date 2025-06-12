import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ItemGroup } from '../models/itemGroup';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class ItemGroupService {
  //* The URL to the API endpoint for item groups. \\
  private readonly apiUrl = environment.apiUrl + 'itemGroup';

  //* getting the current user from the  AuthService\\
  currentUser: any;

  constructor(private http: HttpClient, private authService: AuthService) {
    this.currentUser = this.authService.currentUserValue;
  }

  //* Method for getting all item groups. \\
  getAll(): Observable<ItemGroup[]> {
    return this.http.get<ItemGroup[]>(this.apiUrl);
  }

  //* Method for getting a item group by ID. \\
  findById(Id: number): Observable<ItemGroup> {
    return this.http.get<ItemGroup>(this.apiUrl + '/' + Id);
  }

  //* Method for creating a item group. \\
  create(itemGroup: ItemGroup): Observable<ItemGroup> {
    return this.http.post<ItemGroup>(this.apiUrl, itemGroup);
  }

  //* Method for deleting a item group by ID. \\
  delete(Id: number, note: string): Observable<ItemGroup> {
    return this.http.delete<ItemGroup>(this.apiUrl + '/ArchiveById/' + Id, {
      body: { archiveNote: note },
    });
  }

  //* Method for updating a item group. \\
  update(itemGroup: ItemGroup): Observable<ItemGroup> {
    return this.http.put<ItemGroup>(
      this.apiUrl + '/' + itemGroup.id, itemGroup);
  }
}
