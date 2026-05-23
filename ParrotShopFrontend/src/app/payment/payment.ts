import { HttpClient } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { Environment } from '../global/env';
import { loadStripe, Stripe, StripeElements } from '@stripe/stripe-js';
import { Router } from '@angular/router';

@Component({
  selector: 'app-payment',
  imports: [],
  templateUrl: './payment.html',
  styleUrl: './payment.css',
})
export class Payment {
  env = inject(Environment);
  stripe = signal<Stripe | null>(null);
  elements = signal<StripeElements | null>(null);
  errorMessage = signal<string>('');
  isProcessing = signal<boolean>(false);



  constructor(private http: HttpClient, private router: Router){ }
  async ngOnInit(){
    this.stripe.set(await loadStripe(this.env.STRIPEKEY));
    this.http.post(this.env.APIURI+"/payments/newPaymentIntent", {}, {
      withCredentials: true
    }).subscribe((res:any)=>{
      this.initializeStripeElements(res.clientSecret);
    });

  }

  initializeStripeElements(clientSecret: string) {
    if (!this.stripe) return;
    this.elements.set(this.stripe()!.elements({ clientSecret }));
    const paymentElement = this.elements()!.create('payment');
    paymentElement.mount('#stripe-payment');
  }


  async pay() {
    if (!this.stripe() || !this.elements()) return;

    this.isProcessing.set(true);
    const { error } = await this.stripe()!.confirmPayment({
      elements: this.elements()!,
      confirmParams: {
        return_url: this.env.ANGULARURI + '/checkoutSuccess'
      }
    });
    if (error) {
      this.errorMessage.set(error.message ?? 'An unknown error occurred');
    }
    
    this.isProcessing.set(false);
  }
}
