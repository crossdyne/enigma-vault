import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, firstValueFrom, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Result, AppError, ErrorCode } from '@crossdyne/toolkit';

@Injectable()
export abstract class HttpService {
    protected readonly http = inject(HttpClient);

    protected constructor(protected readonly baseUrl: string) {}

    protected get<T>(path: string, params?: HttpParams): Observable<Result<T>> {
        return this.wrap<T>(this.http.get<T>(`${this.baseUrl}${path}`, { params }));
    }

    protected post<T>(path: string, body: any): Observable<Result<T>> {
        return this.wrap<T>(this.http.post<T>(`${this.baseUrl}${path}`, body));
    }

    protected put<T>(path: string, body: any): Observable<Result<T>> {
        return this.wrap<T>(this.http.put<T>(`${this.baseUrl}${path}`, body));
    }

    protected patch<T>(path: string, body: any): Observable<Result<T>> {
        return this.wrap<T>(this.http.patch<T>(`${this.baseUrl}${path}`, body));
    }

    protected delete<T>(path: string): Observable<Result<T>> {
        return this.wrap<T>(this.http.delete<T>(`${this.baseUrl}${path}`));
    }

    protected getAsync<T>(path: string, params?: HttpParams): Promise<Result<T>> {
        return firstValueFrom(this.get<T>(path, params));
    }

    protected postAsync<T>(path: string, body: any): Promise<Result<T>> {
        return firstValueFrom(this.post<T>(path, body));
    }

    protected putAsync<T>(path: string, body: any): Promise<Result<T>> {
        return firstValueFrom(this.put<T>(path, body));
    }

    protected patchAsync<T>(path: string, body: any): Promise<Result<T>> {
        return firstValueFrom(this.patch<T>(path, body));
    }

    protected deleteAsync<T>(path: string): Promise<Result<T>> {
        return firstValueFrom(this.delete<T>(path));
    }

    protected getWithHeaders<T>(
        path: string,
        headers: HttpHeaders,
        params?: HttpParams
    ): Observable<Result<T>> {
        return this.wrap<T>(this.http.get<T>(`${this.baseUrl}${path}`, { headers, params }));
    }

    protected postWithHeaders<T>(
        path: string,
        body: any,
        headers: HttpHeaders
    ): Observable<Result<T>> {
        return this.wrap<T>(this.http.post<T>(`${this.baseUrl}${path}`, body, { headers }));
    }

    private wrap<T>(source: Observable<T>): Observable<Result<T>> {
        return source.pipe(
            map(response => Result.success<T>(response)),
            catchError((error: HttpErrorResponse): Observable<Result<T>> => {
                if (Array.isArray(error.error)) {
                    const errors = error.error
                        .filter((item: any) => item?.code?.name && item?.code?.code && item?.message)
                        .map((item: any) => AppError.fromJSON(item));

                    if (errors.length > 0) {
                        return of(Result.failure<T>(errors));
                    }
                }

                const message = error.message || `HTTP ${error.status}: ${error.statusText}`;
                const errorCode = this.mapStatusToCode(error.status);

                return of(Result.failure<T>(new AppError(errorCode, message)));
            })
        );
    }

    private mapStatusToCode(status: number): ErrorCode {
        switch (status) {
            case 400: return ErrorCode.BadRequest;
            case 401: return ErrorCode.Unauthorized;
            case 404: return ErrorCode.NotFound;
            case 409: return ErrorCode.Conflict;
            case 0:   return ErrorCode.Connection;
            default:  return status >= 500 ? ErrorCode.Server : ErrorCode.InvalidResponse;
        }
    }
}