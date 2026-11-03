import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const token = sessionStorage.getItem('accessToken');

  // Skip attaching token for unauthenticated login/register calls
  const isAuthCall =
    req.url.includes('/api/Admin/login') ||
    req.url.includes('/api/Admin/Register') ||
    req.url.includes('/api/Auth/login') ||
    req.url.includes('/api/Auth/register');

  if (isAuthCall || !token) {
    return next(req);
  }

  const authReq = req.clone({
    setHeaders: {
      Authorization: `Bearer ${token}`
    }
  });

  return next(authReq);
};
