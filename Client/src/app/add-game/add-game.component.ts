import { Component, OnInit } from '@angular/core';
import { Genre } from '../models/genre';
import { Comment } from '../models/comment';
import { GameService } from '../services/game.service';
import { Game } from '../models/game';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-game',
  templateUrl: './add-game.component.html',
  styleUrls: ['./add-game.component.css']
})
export class AddGameComponent implements OnInit {

  constructor(private gameService : GameService, private router: Router) { }

  ngOnInit(): void {
  }

  onGameCreate(game : {gTitle : string, gDesc : string, gImgUrl : string, gPrice : string}){
    let model : Game = {
      title : game.gTitle,
      description : game.gDesc,
      imageUrl : game.gImgUrl,
      price : Number(game.gPrice),
      comments : [],
      genres : []
    }
    this.gameService.postGame(model).subscribe(res => {
      this.router.navigate(['/games']);
    });
  }

}
