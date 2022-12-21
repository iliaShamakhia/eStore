import { Game } from "./game";

export class OrderDetail{
    id? : number;
    quantity : number = 0;
    price : number = 0;
    gameId : number = 0;
    game? : Game = new Game();
    orderId : number = 0;
}