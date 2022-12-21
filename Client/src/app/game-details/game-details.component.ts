import { Component, OnInit } from '@angular/core';
import { NgModel } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Game } from '../models/game';
import { Genre } from '../models/genre';
import { Comment } from '../models/comment';
import { GameService } from '../services/game.service';
import { GenreService } from '../services/genre.service';
import { AuthenticationService } from '../services/authentication.service';
import { CommentService } from '../services/comment.service';

@Component({
  selector: 'app-game-details',
  templateUrl: './game-details.component.html',
  styleUrls: ['./game-details.component.css']
})
export class GameDetailsComponent implements OnInit {

  game : Game;

  genres : Genre[];

  checkedGenres : (number | undefined)[] = [];

  isCommentAreaActive : boolean = false;

  constructor(private gameService : GameService,
    private genreService : GenreService,
    private route : ActivatedRoute,
    private authService : AuthenticationService,
    private commentService : CommentService) {
      this.game = new Game();
      this.genres = [];
    }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const gameId : number = params['id'];
      this.getGame(gameId);
    });

    this.genreService.getGenres()
    .subscribe(genres => {
      this.genres = genres;
    });

    this.commentService.getRefresh
    .subscribe(() => {
      this.getGame(this.game.id?this.game.id:0);
    });

    this.gameService.getRefresh
    .subscribe(() => {
      this.getGame(this.game.id?this.game.id:0);
    })
  }

  

  onChange($event : any){
    let id = $event.target.value;
    let isChecked = $event.target.checked;
    let genre = this.game.genres.find(g => g.id == id);

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

  updateGame(){
    this.gameService.putGame(this.game)
    .subscribe(game => console.log(game));
  }

  saveImage(gameImgUrl : NgModel){
    this.game.imageUrl = gameImgUrl.value;
    this.updateGame();
  }

  isAuthenticated(){
    return this.authService.isAuthenticated();
  }

  isManager(){
    return this.authService.isManager();
  }

  toggleCommentArea(){
    this.isCommentAreaActive = !this.isCommentAreaActive;
  }

  addComment(parentId : number | undefined, comment : {commentArea : string}){
    let newComment : Comment = {
      body : comment.commentArea,
      isDeleted : false,
      gameId : this.game.id,
      username : this.authService.getUsername(),
      dateAdded : new Date(Date.now()),
      userId : this.authService.getUserId(),
      parentCommentId : parentId,
      childComments : []
    }
    this.commentService.addComment(newComment).subscribe({
      next : (res) => {
        this.toggleCommentArea();
      },
      error : (err) => console.log(err)
    })
  }
  
  getGame(id : number){
    this.gameService
    .getGame(id)
    .subscribe(game => {
      this.game = game;
      this.checkedGenres = this.game.genres.map(g => g.id);
      this.game.comments = this.game.comments.filter(c => c.parentCommentId == null);
    });
  }

}
