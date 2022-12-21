import { Component, OnInit} from '@angular/core';
import { Game } from '../models/game';
import { Order } from '../models/order';
import { OrderDetail } from '../models/orderDetail';
import { AuthenticationService } from '../services/authentication.service';
import { OrderDetailService } from '../services/order-detail.service';
import { OrderService } from '../services/order.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {

  order : Order = new Order();

  constructor(private orderService : OrderService,
    private authService : AuthenticationService,
    private orderDetailService : OrderDetailService) { }

  ngOnInit(): void {
    this.orderService.getRefresh
    .subscribe((order) => {
      this.order = order;
    });
    this.getOrder();
  }

  increase(id? : number){
    if(id){
      this.orderDetailService.increaseQuantity(id)
      .subscribe({
        next : (res) => this.getOrder()
      })
    }
  }

  decrease(detail : OrderDetail){
    if(detail.id && detail.quantity > 1){
      this.orderDetailService.decreaseQuantity(detail.id)
      .subscribe({
        next : (res) => this.getOrder()
      })
    }
  }

  deleteDetail(id? : number){
    if(id){
      this.orderDetailService.delete(id)
      .subscribe({
        next : (res) => this.getOrder()
      })
    }
  }

  getOrder(){
    let userId = this.isAuthenticated()?this.authService.getUserId():"79d052bf-b384-4e69-9556-82d7722d2a37";
    this.orderService.getOrder(userId)
    .subscribe({
      next : (res) => this.order = res
    });
  }
  
  calculateTotal(){
    let total = 0;
    for(let orderDetail of this.order.orderDetails){
      total += this.totalPrice(orderDetail.quantity, orderDetail.game);
    }
    return total;
  }

  totalPrice(quantity : number, game : Game | undefined){
    if(game && game.price){
      return quantity * game.price;
    }
    return 0;
  }

  isAuthenticated(){
    return this.authService.isAuthenticated();
  }
}
