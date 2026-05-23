import { Component, inject, Signal, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Navbar } from './navbar/navbar';
import { CommonModule } from '@angular/common';
import { AuthService } from './services/auth-service';
import { AccDelCard } from './acc-del-card/acc-del-card';
import { CheckoutService } from './services/checkout-service';
import { Footer } from './footer/footer';

export interface User{
  username:string,
  password:string
}

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Navbar, CommonModule, AccDelCard, Footer],
  templateUrl: './app.html',
  styleUrl: './app.css'
})

export class App {
  protected readonly title = signal('LearningAngular');
  public auth = inject(AuthService);
  cartSvc = inject(CheckoutService);


  ngOnInit(){
    this.auth.probe();
    this.cartSvc.getCart().subscribe({
      next: (data)=>{
        this.cartSvc.cart.set(data);
      }
    });
  }
}
