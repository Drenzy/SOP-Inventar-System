import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Item } from '../models/item';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class ItemService {
  //* The URL to the API endpoint for items. \\
  private readonly apiUrl = environment.apiUrl + 'item';

  //* getting the current user from the  AuthService\\
  currentUser: any;

  constructor(private http: HttpClient, private authService: AuthService) {
    this.currentUser = this.authService.currentUserValue;
  }

  //* Method for getting all items.
  getAll(): Observable<Item[]> {
    return this.http.get<Item[]>(this.apiUrl);
  }

  //* Method for getting a item by ID. \\
  findById(Id: number): Observable<Item> {
    return this.http.get<Item>(this.apiUrl + '/' + Id);
  }

  //* Method for creating a item. \\
  create(item: Item): Observable<Item> {
    return this.http.post<Item>(this.apiUrl, item);
  }

  //* Method for deleting a item by ID. \\
  delete(Id: number, note: string): Observable<Item> {
    return this.http.delete<Item>(this.apiUrl + '/ArchiveById/' + Id, {
      body: { archiveNote: note }
    });
  }

  //* Method for updating a item. \\
  update(item: Item): Observable<Item> {
    return this.http.put<Item>(this.apiUrl + '/' + item.id, item);
  }
}
