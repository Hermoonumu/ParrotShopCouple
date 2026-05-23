import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { CheckoutService } from './services/checkout-service';

export const checkoutGuard: CanActivateFn = (route, state) => {
  let cart = inject(CheckoutService);
  let router = inject(Router);
  if (cart.cart().cartItems.length <= 0) {
    router.navigate(["/"]);
    return false;};
  return true;
};
