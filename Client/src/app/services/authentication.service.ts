import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Subject, tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { TokenRequestModel } from '../models/tokenRequestModel';
import { UserAvatarChangeModel } from '../models/userAvatarChangeModel';
import { UserLoginModel } from '../models/userLoginModel';
import { UserRegisterModel } from '../models/userRegisterModel';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
 
  private url : string = "authentication";

  private _refresh$ = new Subject<void>();

  get getRefresh(){
    return this._refresh$;
  }

  constructor(private http : HttpClient,
    private jwtHelper: JwtHelperService) { }

  public register(user : UserRegisterModel){
    return this.http.post(`${environment.baseApiUrl}${this.url}/register`, user);
  }

  public login(user : UserLoginModel){
    return this.http.post(`${environment.baseApiUrl}${this.url}/login`, user);
  }

  public changeAvatar(model : UserAvatarChangeModel){
    return this.http.post(`${environment.baseApiUrl}${this.url}/change-avatar`, model, {
      headers: new HttpHeaders({ 'Authorization': `Bearer ${this.getToken()}` })
    });
  }

  public logOut(){
    localStorage.removeItem("jwt");
    localStorage.removeItem("rToken");
    localStorage.removeItem("fullName");
    localStorage.removeItem("userAvatar");
  }

  public refreshToken(tokenRequest : TokenRequestModel){
    return this.http.post(`${environment.baseApiUrl}${this.url}/refresh-token`, tokenRequest);
  }
  
  public isAuthenticated() : boolean{
    let token = localStorage.getItem("jwt");
    if(token && !this.jwtHelper.isTokenExpired(token)){
      return true;
    }
    let rToken = localStorage.getItem("rToken");
    if(token && rToken && this.jwtHelper.isTokenExpired(token)){
      this.reauthenticate(token, rToken);
      return true;
    }
    return false;
  }

  public reauthenticate(token : string, rToken : string){
    let model : TokenRequestModel = {
      token,
      refreshToken : rToken
    }
    this.refreshToken(model).subscribe({
      next : (res : any) => {
        let newToken = res.token;
        localStorage.setItem("jwt", newToken);
      },
      error : (err) => console.log(err)
    });
  }

  public getUsername() : string{
    let jwt = localStorage.getItem('jwt');
    const userNameClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
    if (jwt != null) {
      let decodedJWT = JSON.parse(window.atob(jwt.split('.')[1]));
      return decodedJWT[userNameClaim];
    }
    return '';
  }

  public getUserId() : string{
    let jwt = localStorage.getItem('jwt');
    const userIdClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
    if (jwt != null) {
      let decodedJWT = JSON.parse(window.atob(jwt.split('.')[1]));
      return decodedJWT[userIdClaim];
    }
    return "";
  }

  public isManagerOrAdmin() : boolean{
    return this.isManager() || this.isAdmin();
  }

  public isManager() : boolean{
    if (this.isAuthenticated()) {
      let jwt = localStorage.getItem('jwt');
      const rolesClaim = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

      if (jwt != null) {
        let decodedJWT = JSON.parse(window.atob(jwt.split('.')[1]));
        var roles = decodedJWT[rolesClaim];

        return roles.includes("Manager");
      }
    }
    return false;
  }

  public isAdmin() : boolean{
    if (this.isAuthenticated()) {
      let jwt = localStorage.getItem('jwt');
      const rolesClaim = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

      if (jwt != null) {
        let decodedJWT = JSON.parse(window.atob(jwt.split('.')[1]));
        var roles = decodedJWT[rolesClaim];
        return roles.includes("Admin");
      }
    }
    return false;
  }

  public getToken(){
    return localStorage.getItem("jwt");
  }

}
