import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { authGuard } from './auth-guard';
import { AuthService } from './services/auth-service';

export const adminguardGuard: CanActivateFn = (route, state) => {
  let auth = inject(AuthService);
  let router = inject(Router);
  if (auth.isAdmin()){
    return true;
  }
  router.navigate([""]);
  return false;
};
