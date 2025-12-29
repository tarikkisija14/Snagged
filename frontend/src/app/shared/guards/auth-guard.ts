import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import{AuthService} from '../../core/services/auth-service/AuthService';

export const authGuard: CanActivateFn = () => {
  console.log('[authGuard] called');
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.isLoggedIn()) {
    console.log('[authGuard] user is logged in');
    return true;
  }

  console.log('[authGuard] user not logged in, redirecting to login');
  router.navigate(['/auth/login']);
  return false;
};
