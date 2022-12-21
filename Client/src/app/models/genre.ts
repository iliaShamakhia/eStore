export class Genre{
    id? : number;
    name : string = "";
    parentGenreId? : number;
    gamesIds : number[] = [];
}