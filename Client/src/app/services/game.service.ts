import { Injectable } from '@angular/core';
import { Game } from '../models/game';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs/internal/Observable';
import { Subject, tap } from 'rxjs';
import { AuthenticationService } from './authentication.service';

@Injectable({
  providedIn: 'root'
})
export class GameService {

  private url : string = "games";

  private _refresh$ = new Subject<void>();

  get getRefresh(){
    return this._refresh$;
  }

  constructor(private http : HttpClient, private authService : AuthenticationService) { }

  public getGames(): Observable<Game[]> {
    return this.http.get<Game[]>(`${environment.baseApiUrl}${this.url}`);
  }

  public getGame(gameId : number) : Observable<Game> {
    return this.http.get<Game>(`${environment.baseApiUrl}${this.url}/${gameId}`);
  }

  public postGame(game : Game){
    return this.http.post(`${environment.baseApiUrl}${this.url}`, game, {
      headers: new HttpHeaders({ 'Authorization': `Bearer ${this.authService.getToken()}` })
    })
    .pipe(
      tap(() => {
        this._refresh$.next();
      })
    );
  }

  public putGame(game : Game){
    return this.http.put(`${environment.baseApiUrl}${this.url}/${game.id}`, game, {
      headers: new HttpHeaders({ 'Authorization': `Bearer ${this.authService.getToken()}` })
    })
    .pipe(
      tap(() => {
        this._refresh$.next();
      })
    );
  }

  public deleteGame(gameId : number | undefined){
    return this.http.delete(`${environment.baseApiUrl}${this.url}/${gameId}`, {
      headers: new HttpHeaders({ 'Authorization': `Bearer ${this.authService.getToken()}` })
    })
    .pipe(
      tap(() => {
        this._refresh$.next();
      })
    );
  }
}
