import { HttpInterceptorFn } from '@angular/common/http';


export const securityHeadersInterceptor: HttpInterceptorFn = (req, next) => {
  const methodsWithBody = ['POST', 'PUT', 'PATCH'];

  if (
    methodsWithBody.includes(req.method.toUpperCase()) &&
    req.body !== null &&
    req.body !== undefined &&
    !req.headers.has('Content-Type')
  ) {
    const cloned = req.clone({
      setHeaders: {
        'Content-Type': 'application/json',
      },
    });
    return next(cloned);
  }

  return next(req);
};
