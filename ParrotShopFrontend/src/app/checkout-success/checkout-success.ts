import { Component, inject, OnDestroy, OnInit, signal } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-checkout-success',
  imports: [],
  templateUrl: './checkout-success.html',
  styleUrl: './checkout-success.css',
})
export class CheckoutSuccess implements OnInit, OnDestroy {
  router = inject(Router);
  seconds = signal(5);
  intId = signal<any>(null); 

  ngOnInit() {
    this.intId.set(setInterval(() => this.countdown(), 1000));
  }

  countdown() {
    if (this.seconds() <= 0) { 
      clearInterval(this.intId());
      this.router.navigate(['/']);
      return;
    }
    
    this.seconds.update((val) => val - 1);
  }

  ngOnDestroy() {
    if (this.intId()) {
      clearInterval(this.intId());
    }
  }
}
