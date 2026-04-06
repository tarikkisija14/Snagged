import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../../services/auth-service/AuthService';

export const authGuard: CanActivateFn = () => {
  const router = inject(Router);
  const authService = inject(AuthService);
  const token = authService.getToken();

  if (!token || token === 'undefined' || token === 'null') {
    router.navigate(['/home/auth/login']);
    return false;
  }

  const userId = authService.getUserId();
  if (userId !== null && !isNaN(userId)) {
    return true;
  }

  router.navigate(['/home/auth/login']);
  return false;
};
