import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const token  = localStorage.getItem('token');

  if (token && token !== 'null') {
    const cloned = req.clone({
      setHeaders: { Authorization: `Bearer ${token}` }
    });

    return next(cloned).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          localStorage.removeItem('token');
          router.navigate(['/home/auth/login']);
        }
        return throwError(() => error);
      })
    );
  }

  return next(req);
};
