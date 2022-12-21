import { User } from "./user";

export class Comment{
    id? : number;
    body : string = "";
    isDeleted : boolean = false;
    gameId? : number;
    username : string = "";
    dateAdded : Date = new Date(Date.now());
    userId : string = "";
    parentCommentId? : number;
    childComments : Comment[] = []; 
}