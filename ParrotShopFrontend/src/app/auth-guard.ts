import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from './services/auth-service';

export const authGuard: CanActivateFn = async (route, state) => {
  const auth   = inject(AuthService);
  const router = inject(Router);
  if (await auth.verifyAuthStatus()) { return true;}

  router.navigate([""]);
  return false;
};

