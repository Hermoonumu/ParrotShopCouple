import { Component, computed, inject, input, signal } from '@angular/core';
import { Parrot } from '../services/parrot-service';
import { Environment } from '../global/env';
import { CurrencyPipe } from '@angular/common';
import { Router } from '@angular/router';
import { Cart, CheckoutService } from '../services/checkout-service';

@Component({
  selector: 'app-item-card',
  imports: [CurrencyPipe],
  templateUrl: './item-card.html',
  styleUrl: './item-card.css',
})
export class ItemCard {
  env = inject(Environment);
  parrot = input<Parrot>();
  router = inject(Router);
  checkout = inject(CheckoutService);
  isInCart = computed(() => {
    const cartItems = this.checkout.cart()?.cartItems || [];
    const parrotId = this.parrot()?.id;
    if (!parrotId) return false;
    return cartItems.some(item => item.itemId == parrotId);
  });
  buttonScript = computed(() => this.isInCart() ? "In Cart" : "To Cart");
  buttonDisabled = computed(() => this.isInCart());

  gotoParrot(){
    this.router.navigate(["/parrot/"+this.parrot()!.id]);
    this.env.selectParrot.set(this.parrot()!);
  }



  onButtonClick(event: MouseEvent) {
    console.log(this.checkout.cart());
    console.log(this.isInCart())
    event.stopPropagation();
    this.checkout.putIntoCart(this.parrot()!.id!).subscribe({
      next: (ct) =>
      {
        console.log(ct);
        this.checkout.cart.update((c) => ({...c, cartItems: [...c!.cartItems, ct] } as Cart));
      }
    });
  }
}
