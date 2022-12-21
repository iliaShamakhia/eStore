import { Component, Input, OnInit } from '@angular/core';
import { Comment } from '../models/comment';
import { AuthenticationService } from '../services/authentication.service';
import { CommentService } from '../services/comment.service';

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.css']
})
export class CommentComponent implements OnInit {

  @Input()
  comment!: Comment;

  isEditCommentAreaActive : boolean = false;

  isReplyCommentAreaActive : boolean = false;

  deletedComments : Comment[] = [];

  constructor(private authService : AuthenticationService,
    private commentService : CommentService) { }

  ngOnInit(): void {
    
  }

  isAuthenticated(){
    return this.authService.isAuthenticated();
  }

  toggleEditCommentArea(){
    this.isEditCommentAreaActive = !this.isEditCommentAreaActive;
  }

  toggleReplyCommentArea(){
    this.isReplyCommentAreaActive = !this.isReplyCommentAreaActive;
  }

  checkUser(userId : string) : boolean{
    return this.authService.getUserId() == userId; 
  }

  replyComment(parentId : number | undefined, gameId : number | undefined, comment : {commentArea : string}){
    let newComment : Comment = {
      body : comment.commentArea,
      isDeleted : false,
      gameId : gameId,
      username : this.authService.getUsername(),
      dateAdded : new Date(Date.now()),
      userId : this.authService.getUserId(),
      parentCommentId : parentId,
      childComments : []
    }
    this.commentService.addComment(newComment).subscribe({
      next : (res) => {
        this.toggleReplyCommentArea();
      },
      error : (err) => console.log(err)
    })
  }

  editComment(comment : Comment, newComment : {editArea : string}){
    comment.body = newComment.editArea;
    this.commentService.editComment(comment).subscribe({
      next : (res) => {
        this.toggleEditCommentArea()
      },
      error : (err) => console.log(err)
    });
  }

  deleteComment(comment : Comment){
    this.commentService.deleteComment(comment.id).subscribe({
      next : (res) => {
        this.deletedComments.push(comment);
      },
      error : (err) => console.log(err)
    });
  }

  restoreComment(comment : Comment){
    let restoredComment = this.deletedComments.find(c => c.id == comment.id);
    if(restoredComment){
      if(restoredComment.childComments.length > 0){
        for(let child of restoredComment.childComments){
          child.id = undefined;
        }
      }
      restoredComment.id = undefined;
      this.commentService.addComment(restoredComment).subscribe({
        next : (res) => {
          this.deletedComments.filter(c => c.id !== comment.id);
        },
        error : (err) => console.log(err)
      });
    }
  }

  isDeleted(comment : Comment){
    let com = this.deletedComments.find(c => c.id == comment.id);
    return com? true : false;
  }

  showCommentDate(date : string){
    let commentDate = Date.parse(date);
    return new Date(date).toDateString();
  }

  isManager(){
    return this.authService.isManager();
  }

}
