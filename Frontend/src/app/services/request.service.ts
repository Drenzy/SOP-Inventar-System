import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Request } from '../models/request';
import { AuthService } from './auth.service';
import { arch } from 'os';

@Injectable({
  providedIn: 'root',
})
export class RequestService {
  //* The URL to the API endpoint for requests. \\
  private readonly apiUrl = environment.apiUrl + 'Request';

  //* getting the current user from the  AuthService\\
  currentUser: any;

  constructor(private http: HttpClient, private authService: AuthService) {
    this.currentUser = this.authService.currentUserValue;
  }

  //* Method for getting all requests. \\
  getAll(): Observable<Request[]> {
    return this.http.get<Request[]>(this.apiUrl);
  }

  //* Method for getting a request by ID. \\
  findById(requestId: number): Observable<Request> {
    return this.http.get<Request>(this.apiUrl + '/' + requestId);
  }

  //* Method for creating a request. \\
  create(request: Request): Observable<Request> {
    return this.http.post<Request>(this.apiUrl, request);
  }

  //* Method for deleting a request by ID. \\
  delete(requestId: number, note: string): Observable<Request> {
    return this.http.request<Request>('DELETE', this.apiUrl + '/' + requestId, {
    body: { archiveNote: note },
    });
  }

  //* Method for updating a request. \\
  update(request: Request): Observable<Request> {
    return this.http.put<Request>(this.apiUrl + '/' + request.id, request);
  }
}
