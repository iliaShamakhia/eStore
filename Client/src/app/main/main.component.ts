import { Component, OnInit } from '@angular/core';
import { NgModel } from '@angular/forms';
import { Router } from '@angular/router';
import { Game } from '../models/game';
import { Genre } from '../models/genre';
import { Order } from '../models/order';
import { OrderDetail } from '../models/orderDetail';
import { AuthenticationService } from '../services/authentication.service';
import { GameService } from '../services/game.service';
import { GenreService } from '../services/genre.service';
import { OrderDetailService } from '../services/order-detail.service';
import { OrderService } from '../services/order.service';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {

  games : Game[] = [];

  genres : Genre[];

  chosenGenres : string[];

  filteredGames : Game[] = [];

  gameToDeleteId : number | undefined;

  order : Order = new Order();

  constructor(private gameService : GameService,
    private genreService : GenreService,
    private router: Router,
    private authService : AuthenticationService,
    private orderService : OrderService,
    private orderDetailService : OrderDetailService){
    this.genres = [];
    this.chosenGenres = [];
    this.gameToDeleteId = -1;
  }

  ngOnInit() : void{

    this.getGames();
    this.getGenres();
    this.getOrder();

    this.gameService.getRefresh
    .subscribe(() => {
      this.getGames();
    })
  }

  isAuthenticated(): boolean{
    return this.authService.isAuthenticated();
  }

  isManagerOrAdmin() : boolean{
    return this.authService.isManagerOrAdmin();
  }

  getGenres(){
    this.genreService
    .getGenres()
    .subscribe((result : Genre[]) => {
      this.genres = result;
    });
  }

  getGames(){
    this.gameService
    .getGames()
    .subscribe((result : Game[]) => {
      this.games = result;
      this.filteredGames = result;
    });
  }

  onChange($event : any){
    let isChecked = $event.target.checked;
    let name = $event.target.name;
    let isChosen = this.chosenGenres.includes(name);

    if(!isChosen && isChecked){
      this.chosenGenres.push(name);
      this.searchByGenre();
    }
    if(isChosen && !isChecked){
      this.chosenGenres = this.chosenGenres.filter(g => g !== name);
      this.searchByGenre();
    }
  }

  searchByName(searchWord : NgModel){
    if(searchWord.value.length >= 3){
      this.filteredGames = this.games.filter(g => g.title.toLowerCase().includes(searchWord.value.toLowerCase()));
    }else{
      this.filteredGames = this.games;
    }
  }

  searchByGenre(){
    if(this.chosenGenres.length == 0){
      this.filteredGames = this.games;
      return;
    }
    if(this.filteredGames.length <= 1){
      this.filteredGames = this.games;
      for(let gnr of this.chosenGenres){
        this.filteredGames = this.filteredGames.filter(g => g.genres.map(gn => gn.name).includes(gnr));
      }
    }else{
      for(let gnr of this.chosenGenres){
        this.filteredGames = this.filteredGames.filter(g => g.genres.map(gn => gn.name).includes(gnr));
      }
    }
    
  }

  setGameId(gameId : number | undefined){
    this.gameToDeleteId = gameId;
  }

  deleteGame(){
    this.gameService.deleteGame(this.gameToDeleteId)
    .subscribe(res => {
      this.router.navigate(['/games']);
    });
  }
  
  getOrder(){
    let userId = this.isAuthenticated()?this.authService.getUserId():"79d052bf-b384-4e69-9556-82d7722d2a37";
    this.orderService.getOrder(userId)
      .subscribe({
        next: (res) => {
          if (res != null) {
            this.order = res;
          }
        },
        error: (err) => console.log(err)
      })
  }

  addToCart(game : Game){
    let orderDetail = this.order.orderDetails.find(od => (od.orderId == this.order.id) && (od.gameId == game.id));
    if(orderDetail){
      this.orderDetailService.increaseQuantity(orderDetail.id?orderDetail.id:0)
      .subscribe({
        next : (res) => {
          console.log("increased");
        },
        error : (err) => console.log(err)
      })
    }else{
      let newOrderDetail : OrderDetail = {
        quantity : 1,
        price : game.price,
        gameId : game.id?game.id:0,
        orderId : this.order.id?this.order.id:0
      } 
      this.orderDetailService.addOrderDetail(newOrderDetail)
      .subscribe({
        next : (res) => {
          console.log("added");
          this.getOrder();
        },
        error : (err) => console.log(err)
      })
    }
  }
}
