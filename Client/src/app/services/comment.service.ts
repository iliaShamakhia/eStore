import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Subject, tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Comment } from '../models/comment';
import { AuthenticationService } from './authentication.service';

@Injectable({
  providedIn: 'root'
})
export class CommentService {

  private url : string = "comments";

  private _refresh$ = new Subject<void>();

  get getRefresh(){
    return this._refresh$;
  }

  constructor(private http : HttpClient, private authService : AuthenticationService) { }

  public addComment(comment : Comment){
    return this.http.post(`${environment.baseApiUrl}${this.url}`, comment, {
      headers: new HttpHeaders({ 'Authorization': `Bearer ${this.authService.getToken()}` })
    })
    .pipe(
      tap(() => {
        this._refresh$.next();
      })
    );
  }

  public editComment(comment : Comment){
    return this.http.put(`${environment.baseApiUrl}${this.url}/${comment.id}`, comment, {
      headers: new HttpHeaders({ 'Authorization': `Bearer ${this.authService.getToken()}` })
    });
  }

  public deleteComment(id? : number){
    return this.http.delete(`${environment.baseApiUrl}${this.url}/${id}`, {
      headers: new HttpHeaders({ 'Authorization': `Bearer ${this.authService.getToken()}` })
    });
  }
}
