import { Component, computed, inject, signal } from '@angular/core';
import { Color, Parrot, ParrotService } from '../services/parrot-service';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { Environment } from '../global/env';
import { CurrencyPipe } from '@angular/common';
import { ParrotTraitShow } from '../parrot-trait-show/parrot-trait-show';
import { CheckoutService } from '../services/checkout-service';
import { Cart } from '../services/checkout-service';
@Component({
  selector: 'app-parrot-show',
  imports: [CurrencyPipe, ParrotTraitShow],
  templateUrl: './parrot-show.html',
  styleUrl: './parrot-show.css',
})
export class ParrotShow {
  parrot = signal<Parrot>({} as Parrot);
  parrotService = inject(ParrotService);
  checkout = inject(CheckoutService);
  env = inject(Environment);
  colours: Record<number, string> = {};
    isInCart = computed(() => {
    const cartItems = this.checkout.cart()?.cartItems || [];
    const parrotId = this.parrot()?.id;
    if (!parrotId) return false;
    return cartItems.some(item => item.itemId == parrotId);
  });
  buttonScript = computed(() => this.isInCart() ? "In Cart" : "To Cart");
  buttonDisabled = computed(() => this.isInCart());




  constructor(private ar: ActivatedRoute){
    this.parrot.set(this.env.selectParrot()!);
    let colourNums = Object.values(Color).slice(14);
    let colourNames = Object.values(Color).slice(0, 14);
    for (let i = 0; i < colourNames.length; i++){
      this.colours[colourNums[i] as number] = colourNames[i] as string;
    }
    if (!this.parrot()){
      this.ar.paramMap.subscribe((params: ParamMap) => {
        this.parrotService.getSingularParrot(parseInt(params.get('id')!)).subscribe({
          next: (data)=>{
            this.parrot.set(data);
            console.log(this.parrot());
          }
        });
      });
    }
  }

  toCart(){
    this.checkout.putIntoCart(this.parrot()!.id!).subscribe({
      next: (ct) =>
      {
        this.checkout.cart.update((c) => ({...c, cartItems: [...c.cartItems, ct] } as Cart));
      }
    });
  }

  get currentColors(): string[] {
    let colors: string[] = String(this.parrot().colorType).split(',');
    return colors.map(x=> x.trim());
  }

  textCol(s: string){
    let str = s.toLowerCase();
    if (str == "black" || str == "gray" || str=="brown" ||str=="purple"|| str=="green" || str=="red" || str=="colourful"
    ){
      return "white"
    } else {
      return "black";
    }
  }

  getBackgroundCol(c: string){
    let str = c.toLowerCase();
    if (str=="colourful"){
      return "linear-gradient(90deg, rgb(169, 0, 0) 0%,rgb(151, 90, 0) 10%,rgb(151, 161, 0) 20%,rgb(5, 144, 0) 30%,rgb(37, 136, 134) 40%,rgb(27, 121, 136) 50%,rgb(17, 75, 141) 60%,rgb(52, 6, 144) 70%,rgb(106, 5, 143) 80%,rgb(148, 5, 129) 90%,rgb(150, 4, 4) 100%)"
    }
    return c;
  }

}
