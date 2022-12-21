import { OrderDetail } from "./orderDetail";

export class Order{
    id? : number;
    userId : string = "";
    orderDetails : OrderDetail[] = [];
}