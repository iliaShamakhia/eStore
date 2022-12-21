import { Component, OnInit } from '@angular/core';
import { Order } from '../models/order';
import { AuthenticationService } from '../services/authentication.service';
import { OrderService } from '../services/order.service';
import { OrderDetailService } from '../services/order-detail.service';

@Component({
  selector: 'app-complete-order',
  templateUrl: './complete-order.component.html',
  styleUrls: ['./complete-order.component.css']
})
export class CompleteOrderComponent implements OnInit {

  order : Order = new Order();

  constructor(private orderService : OrderService,
    private authService : AuthenticationService,
    private orderDetailService : OrderDetailService) { }

  ngOnInit(): void {
    if(!this.authService.isAuthenticated()){
      this.getOrder();
    }
  }

  getOrder(){
    this.orderService.getOrder("79d052bf-b384-4e69-9556-82d7722d2a37")
      .subscribe({
        next : (res) => this.order = res
      })
  }

  completeOrder(){
    if(this.order.id){
      for(let od of this.order.orderDetails){
        if(od.id){
          this.orderDetailService.delete(od.id)
          .subscribe({
            next : () => console.log("deleted")
          })
        }
      }
    }
    
  }

}
