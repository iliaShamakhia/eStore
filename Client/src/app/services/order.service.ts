import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject, tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Order } from '../models/order';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  private url : string = "orders";

  private _refresh$ = new Subject<Order>();

  get getRefresh(){
    return this._refresh$;
  }

  constructor(private http : HttpClient) { }

  public getOrder(userId : string) : Observable<Order>{
    return this.http.get<Order>(`${environment.baseApiUrl}${this.url}/${userId}`)
    .pipe(
      tap((res) => {
        this._refresh$.next(res);
      })
    );
  }

  public createOrder(order : Order){
    return this.http.post(`${environment.baseApiUrl}${this.url}`, order)
    /* .pipe(
      tap((res) => {
        this._refresh$.next(res);
      })
    ); */
  }

  public deleteOrder(id : number){
    return this.http.delete(`${environment.baseApiUrl}${this.url}/${id}`);
  }
}
