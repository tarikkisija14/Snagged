import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';

const CSRF_EXCLUDED_PATHS = ['/auth/login', '/auth/register'];

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const token  = localStorage.getItem('token');

  const isCsrfExcluded = CSRF_EXCLUDED_PATHS.some(p =>
    req.url.toLowerCase().includes(p)
  );

  if (token && token !== 'null' && token !== 'undefined') {
    const headers: Record<string, string> = {
      Authorization: `Bearer ${token}`,
    };

    if (!isCsrfExcluded) {
      headers['X-CSRF-TOKEN'] = token; // ← was 'X-Requested-With': 'XMLHttpRequest'
    }

    const cloned = req.clone({ setHeaders: headers });

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

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => throwError(() => error))
  );
};
