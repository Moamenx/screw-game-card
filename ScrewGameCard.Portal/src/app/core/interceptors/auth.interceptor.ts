import { HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);

  if(isAuthRequest(req) || isPublicRequest(req) || isRefreshRequest(req)) {
        return next(req);
}
  const token = authService.getToken();

  if (token) {
    const cloned = req.clone({
      headers: req.headers.set('Authorization', `Bearer ${token}`)
    });

    return next(cloned).pipe(
      catchError((error) => {
        if (error.status === 401) {
          // Token might be expired, try to refresh
          return authService.refreshToken().pipe(
            switchMap(() => {
              const newToken = authService.getToken();
              if (newToken) {
                const retryReq = req.clone({
                  headers: req.headers.set('Authorization', `Bearer ${newToken}`)
                });
                return next(retryReq);
              } else {
                authService.logout();
                return throwError(() => error);
              }
            }),
            catchError(() => {
              authService.logout();
              return throwError(() => error);
            })
          );
        }
        return throwError(() => error);
      })
    );
  }

  return next(req);
};

function isAuthRequest(req: HttpRequest<unknown>): boolean {
  return req.url.includes('/auth/');
}

function isPublicRequest(req: HttpRequest<unknown>): boolean {
  return req.url.includes('/public/') || req.url.includes('/assets/');
}

function isRefreshRequest(req: HttpRequest<unknown>): boolean {
  return req.url.includes('/auth/refresh');
}

