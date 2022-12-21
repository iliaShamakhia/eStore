import { Component, OnInit } from '@angular/core';
import { Game } from '../models/game';
import { GameService } from '../services/game.service';
import { ActivatedRoute, Router } from '@angular/router';
import { GenreService } from '../services/genre.service';
import { Genre } from '../models/genre';

@Component({
  selector: 'app-edit-game',
  templateUrl: './edit-game.component.html',
  styleUrls: ['./edit-game.component.css']
})
export class EditGameComponent implements OnInit {

  game : Game;

  genres : Genre[];

  checkedGenres : (number | undefined)[] = [];

  constructor(
    private gameService : GameService,
    private genreService : GenreService,
    private route : ActivatedRoute,
    private router: Router) {
    this.game = new Game();
    this.genres = [];
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const gameId : number = params['id'];
      this.gameService
      .getGame(gameId)
      .subscribe(game => {
        this.game = game;
        this.checkedGenres = this.game.genres.map(g => g.id);
      });
    });

    this.genreService.getGenres()
    .subscribe(genres => {
      this.genres = genres;
    });
  }

  onChange($event : any){
    let id = $event.target.value;
    let isChecked = $event.target.checked;
    let genre = this.game.genres.find(g => g.id == id);

    if(genre && isChecked){
      return;
    }
    if(!genre && isChecked){
      let newGenre : Genre = {
        id : id,
        name : $event.target.name,
        gamesIds : []
      };
      this.game?.genres.push(newGenre);
    }
    if(genre && !isChecked){
      if(this.game){
        this.game.genres = this.game.genres.filter(g => g.id !== genre?.id);
      }
    }
  }

  onGameEdit(){
    this.gameService.putGame(this.game)
    .subscribe(res => {
      this.router.navigate(['/']);
    });
  }

}
