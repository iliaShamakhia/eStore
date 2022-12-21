import { Genre } from "./genre";
import { Comment } from "./comment";

export class Game{
    id? : number;
    title : string = "";
    description : string = "";
    price : number = 0;
    imageUrl : string = "";
    comments : Comment[]= [];
    genres : Genre[] = [];
}