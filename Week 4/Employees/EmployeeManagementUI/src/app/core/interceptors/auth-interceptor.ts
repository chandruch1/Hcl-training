
import {
  HttpInterceptorFn,
  HttpErrorResponse
} from '@angular/common/http';

import { inject } from '@angular/core';
import { catchError, switchMap, throwError } from 'rxjs';

import { Token } from '../services/token';
import { Auth } from '../services/auth';

export const authInterceptor: HttpInterceptorFn = (req, next) => {

  const tokenService = inject(Token);
  const authService = inject(Auth);

  const accessToken = tokenService.getAccessToken();

  const isBackendRequest =
    req.url.startsWith('http://localhost:5000/api/');

  const isAuthRequest =
    req.url.endsWith('/Auth/login') ||
    req.url.endsWith('/Auth/register') ||
    req.url.endsWith('/Auth/refresh');

  // Attach access token
  const authReq = accessToken && isBackendRequest
    ? req.clone({
        setHeaders: {
          Authorization: `Bearer ${accessToken}`
        }
      })
    : req;

  return next(authReq).pipe(

    catchError((error: HttpErrorResponse) => {

      // Handle only 401 errors from protected API requests
      if (
        error.status !== 401 ||
        !isBackendRequest ||
        isAuthRequest
      ) {
        return throwError(() => error);
      }

      const refreshToken = tokenService.getRefreshToken();

      if (!refreshToken) {
        tokenService.clearTokens();
        return throwError(() => error);
      }

      // Request new tokens
      return authService.refreshToken({ refreshToken }).pipe(

        switchMap((response) => {

          // Save new tokens
          tokenService.saveTokens(
            response.accessToken,
            response.refreshToken
          );

          // Retry original request with new access token
          const retryRequest = req.clone({
            setHeaders: {
              Authorization: `Bearer ${response.accessToken}`
            }
          });

          return next(retryRequest);
        }),

        catchError((refreshError) => {
          tokenService.clearTokens();
          return throwError(() => refreshError);
        })

      );
    })

  );
};
