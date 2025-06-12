import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Room } from '../models/room';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class RoomService {
  //* The URL to the API endpoint for rooms. \\
  private readonly apiUrl = environment.apiUrl + 'Room';

  //* getting the current user from the  AuthService\\
  currentUser: any;

  constructor(private http: HttpClient, private authService: AuthService) {
    this.currentUser = this.authService.currentUserValue;
  }

  //* Method for getting all rooms. \\
  getAll(): Observable<Room[]> {
    return this.http.get<Room[]>(this.apiUrl);
  }

  //* Method for getting a room by ID. \\
  findById(roomId: number): Observable<Room> {
    return this.http.get<Room>(this.apiUrl + '/' + roomId);
  }

  //* Method for creating a room. \\
  create(room: Room): Observable<Room> {
    return this.http.post<Room>(this.apiUrl, room);
  }

  //* Method for deleting a room by ID. \\
  delete(roomId: number): Observable<Room> {
    return this.http.delete<Room>(this.apiUrl + '/' + roomId);
  }

  //* Method for updating a room. \\
  update(room: Room): Observable<Room> {
    return this.http.put<Room>(this.apiUrl + '/' + room.id, room);
  }
}
