import { Component, inject, input, output, signal } from '@angular/core';
import { CheckoutService, Order } from '../services/checkout-service';
import { CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-order-card',
  imports: [CurrencyPipe],
  templateUrl: './order-card.html',
  styleUrl: './order-card.css',
})
export class OrderCard {
  order = input<Order>();
  isExpanded = signal(false);
  admin = input<number>();
  newStatus = signal("");
  orderSvc = inject(CheckoutService);
  statusChanged = output();

  ngOnInit(){
    this.newStatus.set(this.order()!.status);
  }
  change(e:Event){
    this.newStatus.set((e.target as HTMLInputElement).value);
    console.log(this.newStatus());
  }

  submit(){
    this.orderSvc.alterOrderStatus(this.newStatus(), this.order()!.id);
    this.statusChanged.emit();
  }
}
