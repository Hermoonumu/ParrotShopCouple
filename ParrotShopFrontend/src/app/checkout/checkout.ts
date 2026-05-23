import { Component, computed, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthService } from '../services/auth-service';
import { Payment } from '../payment/payment';
import { CheckoutService } from '../services/checkout-service';
import { CurrencyPipe } from '@angular/common';
import { ItemCard } from '../item-card/item-card';
import { Environment } from '../global/env';


@Component({
  selector: 'app-checkout',
  imports: [RouterLink, Payment, CurrencyPipe, ItemCard],
  templateUrl: './checkout.html',
  styleUrl: './checkout.css'
})
export class Checkout {
  status = signal(0);
  auth = inject(AuthService);
  cart = inject(CheckoutService);
  env = inject(Environment);

  total = computed(() => {
    let total = 0;
    for (let item of this.cart.cart().cartItems) {
      total += item.item.price!;
    }
    return total;
  });

  sOrNot(){
    var len = this.cart.cart().cartItems.length.toString();
    var number = len.substring(len.length-1);
    if (number != '1'){
      return 's';
    }
    return '';
  }

}

