import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ItemType } from '../models/itemType';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class ItemTypeService {
  private readonly apiUrl = environment.apiUrl + 'ItemType';


  currentUser: any;

  constructor(private http: HttpClient, private authService: AuthService) {
    this.currentUser = this.authService.currentUserValue;
  }

  //* Method for getting all item types. \\
  getAll(): Observable<ItemType[]> {
    return this.http.get<ItemType[]>(this.apiUrl);
  }

  //* Method for getting a item type by ID. \\
  findById(itemTypeId: number): Observable<ItemType> {
    return this.http.get<ItemType>(this.apiUrl + '/' + itemTypeId);
  }

  //* Method for creating a item type. \\
  create(itemType: ItemType): Observable<ItemType> {
    return this.http.post<ItemType>(this.apiUrl, itemType);
  }

  //* Method for deleting a item type by ID. \\
  delete(itemTypeId: number, note: string): Observable<ItemType> {
    return this.http.delete<ItemType>(
      this.apiUrl + '/ArchiveById/' + itemTypeId,
      {
        body: { archiveNote: note }
      }
    );
  }
}
