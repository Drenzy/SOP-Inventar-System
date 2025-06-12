import { inject } from '@angular/core';
import {
  HttpRequest,
  HttpHandlerFn,
  HttpInterceptorFn,
} from '@angular/common/http';
import { AuthService } from '../services/auth.service';

export const JwtInterceptor: HttpInterceptorFn = (
  request: HttpRequest<unknown>,
  next: HttpHandlerFn
) => {
  const authService = inject(AuthService);
  const currentUser = authService.currentUserValue;

  // Skip attaching the token for the extend-token endpoint.
  if (!request.url.endsWith('extend-token') && currentUser?.token) {
    request = request.clone({
      setHeaders: { Authorization: `Bearer ${currentUser.token}` },
    });
  }
  return next(request);
};