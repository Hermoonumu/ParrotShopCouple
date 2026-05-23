import { Component, computed, inject, signal } from '@angular/core';
import { CheckoutService } from '../services/checkout-service';
import { Environment } from '../global/env';
import { CurrencyPipe } from '@angular/common';
import { Router, RouterLink } from "@angular/router";
import { Payment } from '../payment/payment';

@Component({
  selector: 'app-cart',
  imports: [CurrencyPipe, RouterLink],
  templateUrl: './cart.html',
  styleUrl: './cart.css',
})
export class Cart {
  cart = inject(CheckoutService);
  env = inject(Environment);
  router = inject(Router);
  buttonText = computed(() => {
    if (this.cart.cart().cartItems.length){
      return "To Checkout";
    }
    return "Cart is Empty";
  })
  

  get TotalPrice(){
    let total = 0;
    for(let item of this.cart.cart().cartItems){
      total += item.item.price!;
    }
    return total;
  }

  removeFromCart(id: number){
    this.cart.removeFromCart(id).subscribe({
      next:()=>{
        this.cart.cart.update((c)=>({...c, cartItems: c.cartItems.filter(x=>x.itemId!=id)}));
      }
    });
  }
}
