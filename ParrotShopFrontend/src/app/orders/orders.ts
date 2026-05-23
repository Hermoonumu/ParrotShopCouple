import { Component, inject, signal } from '@angular/core';
import { OrderCard } from '../order-card/order-card';
import { CheckoutService, Order } from '../services/checkout-service';

@Component({
  selector: 'app-orders',
  imports: [OrderCard],
  templateUrl: './orders.html',
  styleUrl: './orders.css',
})
export class Orders {
  checkout = inject(CheckoutService);
  orders = signal<Order[]>([]);

  ngOnInit() {
    this.checkout.getOrders().subscribe({
      next: (data) => {
        this.orders.set(data);
      },
    });
  }

}
