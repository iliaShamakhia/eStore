import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Genre } from '../models/genre';

@Injectable({
  providedIn: 'root'
})
export class GenreService {

  private url : string = "genres";

  constructor(private http : HttpClient) { }

  public getGenres(): Observable<Genre[]> {
    return this.http.get<Genre[]>(`${environment.baseApiUrl}${this.url}`);
  }
}
