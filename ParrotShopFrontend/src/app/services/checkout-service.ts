import { inject, Injectable, signal } from '@angular/core';
import { Parrot } from './parrot-service';
import { HttpClient } from '@angular/common/http';
import { Environment } from '../global/env';
import { Observable } from 'rxjs';

export interface Cart{
  id:number;
  userId:number;
  cartItems:CartItem[];
}

export interface CartItem{
  id: number;
  cartId:number;
  itemId:number;
  item: Parrot;
  qty:number;
}

export interface Order{
  id:number;
  userId:number;
  status:string;
  orderItems:OrderItem[];
  total:number;
  timestamp:Date;
  shippingaddress:string;
}

export interface OrderItem{
  id:number;
  linkedItemId:number;
  linkedItem:Parrot;
  qty:number;
  description:string;
  priceAtOrderTime:number;
  imageUrl:string;
  orderId:number;
}

@Injectable({
  providedIn: 'root',
})

export class CheckoutService {
  env = inject(Environment);
  cart = signal<Cart>({id:-1, userId:-1, cartItems:[] as CartItem[]});

  constructor(private http: HttpClient){ }


  getCart():Observable<Cart>{
    return this.http.get<Cart>(this.env.APIURI+"/cart", {withCredentials: true});
  }

  getOrders():Observable<Order[]>{
    return this.http.get<Order[]>(this.env.APIURI+"/orders", {withCredentials: true});
  }

  putIntoCart(id:number):Observable<CartItem>{
    return this.http.post<CartItem>(this.env.APIURI+"/cart", {}, {withCredentials: true, params:{
      ItemId: id,
      qty: 1
    }});
  }

  removeFromCart(id: number):Observable<any>{
    return this.http.delete(this.env.APIURI+"/cart", {withCredentials: true, params:{
      ItemId:id
    }});
  }

  onLogout(){
    this.cart.set({id:-1, userId:-1, cartItems:[] as CartItem[]});
  }

  getPendingOrders(){
    return this.http.get<Order[]>(this.env.APIURI+"/orders/pending", {withCredentials: true});
  }

  getAllOrders(){
    return this.http.get<Order[]>(this.env.APIURI+"/orders/all", {withCredentials: true});
  }

  alterOrderStatus(s : string, id:number){
    this.http.patch(this.env.APIURI+"/orders",{status:s, orderId:id}, {withCredentials: true}).subscribe();
  }
}
