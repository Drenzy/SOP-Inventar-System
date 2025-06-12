import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideClientHydration } from '@angular/platform-browser';
import { routes } from './app-routing.module';
import { provideHttpClient, withInterceptors, withInterceptorsFromDi } from '@angular/common/http';
import { JwtInterceptor } from './helper/jwt.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [provideRouter(routes), 
    provideClientHydration(),
    provideHttpClient(
      withInterceptors([JwtInterceptor]),
      withInterceptorsFromDi()
    ),
  ],
};
