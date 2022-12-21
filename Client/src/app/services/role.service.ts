import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject, tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { UserWithRole } from '../models/userWithRole';
import { AuthenticationService } from './authentication.service';

@Injectable({
  providedIn: 'root'
})
export class RoleService {

  private url : string = "roles";

  constructor(private http : HttpClient, private authService : AuthenticationService) { }

  public getRoles(): Observable<UserWithRole[]>{
    return this.http.get<UserWithRole[]>(`${environment.baseApiUrl}${this.url}/get-users`, {
      headers: new HttpHeaders({ 'Authorization': `Bearer ${this.authService.getToken()}` })
    });
  }

  public changeRole(username : string, role : string){
    return this.http.put(`${environment.baseApiUrl}${this.url}/user/${username}/add/${role}`, {
      headers: new HttpHeaders({ 'Authorization': `Bearer ${this.authService.getToken()}` })
    });
  }

}
