import { Component, inject, signal } from '@angular/core';
import { CheckoutService, Order } from '../services/checkout-service';
import { OrderCard } from '../order-card/order-card';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-order-mgmt',
  imports: [OrderCard, FormsModule],
  templateUrl: './order-mgmt.html',
  styleUrl: './order-mgmt.css',
})
export class OrderMgmt {
  orderSvc = inject(CheckoutService);
  orders = signal<Order[]>([]);
  pendingNotAll = signal<boolean>(true);

  ngOnInit(){
    this.orderSvc.getPendingOrders().subscribe({
      next: (data)=>{
        this.orders.set(data);
      }
    });
  }

  switch(e:Event){
    this.pendingNotAll.set((e.target as HTMLInputElement).value == 'true');
    this.retrieve();
  }
  retrieve(){
        if (this.pendingNotAll()){
          this.orderSvc.getPendingOrders().subscribe({
      next: (data)=>{
        this.orders.set(data);
      }
    });
    }
    else {
      this.orderSvc.getAllOrders().subscribe({
        next: (data)=>{
          this.orders.set(data);
        }
      });
    }
  }
  update(){
    this.retrieve();
  }
}
