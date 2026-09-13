import { HttpInterceptorFn } from "@angular/common/http";
import { environment } from "../../environments/environment";

export const bffBaseUrlInterceptor: HttpInterceptorFn = (req, next) => {

    if (req.url.startsWith('http://') || req.url.startsWith('https://')) {
        return next(req);
    }

    const baseUrl = environment.bffBaseUrl.replace(/\/$/, '');
    const requestPath = req.url.replace(/^\//, '');

    const apiRequest = req.clone({
        url: `${baseUrl}/${requestPath}`
    });

    return next(apiRequest);
};