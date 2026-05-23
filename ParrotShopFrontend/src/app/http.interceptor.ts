import { HttpInterceptorFn, HttpErrorResponse, HttpClient } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, switchMap, throwError } from 'rxjs';
import { Environment } from './global/env';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  let env = inject(Environment);
  let http = inject(HttpClient);
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401 && !req.url.includes('/refresh')){
        return http.post(env.APIURI+"/auth/refresh", {}, {withCredentials: true}).pipe(
          switchMap(() => {
            return next(req);
          })
        );
      }
      return throwError(() => error);
    })
  );
};