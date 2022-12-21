import { Component, OnInit } from '@angular/core';
import { NgModel } from '@angular/forms';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { Order } from '../models/order';
import { UserAvatarChangeModel } from '../models/userAvatarChangeModel';
import { AuthenticationService } from '../services/authentication.service';
import { OrderService } from '../services/order.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  err : any | undefined;

  fullName : string | null = "";

  imageUrl : string = "";

  order : Order = new Order();

  constructor(private authService : AuthenticationService,
    private router: Router,
    private orderService : OrderService) { }

  ngOnInit(): void {
    if(!this.isAuthenticated()){
      this.getOrder();
    }
    this.getUserInfo();

    this.orderService.getRefresh
    .subscribe((order) => {
      this.order = order;
    });
  }

  getOrder(){
    if(this.isAuthenticated()){
      let userId = this.authService.getUserId();
      this.orderService.getOrder(userId)
      .subscribe({
        next : (res) => {
          if(res == null){
            this.createOrder(userId);
          }else{
            this.order = res;
            console.log(res);
          }
        },
        error : (err) => console.log(err)
      })
    }else{
      this.orderService.getOrder("79d052bf-b384-4e69-9556-82d7722d2a37")
      .subscribe({
        next : (res) => this.order = res
      })
    }
  }

  createOrder(userId : string){
    let newOrder : Order = {
      userId,
      orderDetails : []
    }
    this.orderService.createOrder(newOrder)
    .subscribe({
      next : (res) => {
        this.getOrder();
      },
      error : (err) => console.log(err)
    })
  }

  getUserInfo(){
    if(this.isAuthenticated()){
      this.fullName = localStorage.getItem("fullName");
      this.imageUrl = localStorage.getItem("userAvatar") || "";
    }
  }

  logIn(user : any){
    if(!user.username || !user.password){
      this.err = "username and/or password required";
      return;
    }
    this.authService.login(user)
    .subscribe({
      next : (res : any) => {
        let token = res.tokenValue.token;
        let rToken = res.tokenValue.refreshToken;
        if(user.remember){
          localStorage.setItem("jwt", token);
          localStorage.setItem("rToken", rToken);
        }
        localStorage.setItem("jwt",token);
        this.fullName = `${res.user.firstName} ${res.user.lastName}`;
        localStorage.setItem("fullName", this.fullName);
        this.imageUrl = res.user.imageUrl;
        localStorage.setItem("userAvatar", this.imageUrl);
        this.getOrder();
        $("#cancel").trigger('click');
      },
      error : (e) => {console.log(e); this.err = e}
    });
  }

  logOut(){
    this.authService.logOut();
    this.router.navigate(["/"]);
  }

  isAuthenticated(): boolean{
    return this.authService.isAuthenticated();
  }

  isAdmin(){
    return this.authService.isAdmin();
  }

  changeAvatar(newImage : NgModel){
    if(this.isAuthenticated()){
      let username = this.authService.getUsername();
      let model : UserAvatarChangeModel = {
        username,
        imageUrl : newImage.value
      }
      this.authService.changeAvatar(model).subscribe({
        next : (res) => {
          this.imageUrl = newImage.value;
          localStorage.setItem("userAvatar", newImage.value);
          this.router.navigate(["/"]);
        },
        error : (err) => console.log(err)
      });
    }
  }
}
