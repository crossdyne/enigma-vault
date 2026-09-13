import { HttpInterceptorFn, HttpErrorResponse } from "@angular/common/http";
import { catchError, throwError } from "rxjs";
import { environment } from "../../environments/environment";

export const authErrorInterceptor: HttpInterceptorFn = (req, next) => {

    if (req.headers.has('X-Skip-Auth-Interceptor')) {
        return next(req);
    }

    return next(req).pipe(
        catchError((error: HttpErrorResponse) => {
            if (error.status === 401) {
                const currentUrl = window.location.href;
                const returnUrl = encodeURIComponent(currentUrl);
                window.location.href = `${environment.returnAuthUrlBase}?returnUrl=${returnUrl}`;
            }

            return throwError(() => error);
        })
    );
};