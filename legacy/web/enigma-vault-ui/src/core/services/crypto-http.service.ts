import { Injectable } from "@angular/core";
import { HttpService } from "../http/http.service";
import { Result } from "@crossdyne/toolkit";
import { DekResponse } from "../contracts/crypto/dek.response";

@Injectable({
    providedIn: 'root'
})
export class CryptoHttpService extends HttpService {

    constructor() {
        super('api/v1')
    }

    async getDekAsync(): Promise<Result<DekResponse>> {
        return await this.getAsync('/private/dek')
    }
}