import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { OrderDetail } from '../models/orderDetail';

@Injectable({
  providedIn: 'root'
})
export class OrderDetailService {

  private url : string = "orderdetails";

  constructor(private http : HttpClient) { }

  public increaseQuantity(id : number){
    return this.http.put(`${environment.baseApiUrl}${this.url}/${id}/increase`,null);
  }

  public decreaseQuantity(id : number){
    return this.http.put(`${environment.baseApiUrl}${this.url}/${id}/decrease`,null);
  }

  public addOrderDetail(orderDetail : OrderDetail){
    return this.http.post(`${environment.baseApiUrl}${this.url}`, orderDetail)
  }

  public delete(id : number){
    return this.http.delete(`${environment.baseApiUrl}${this.url}/${id}`)
  }
}
