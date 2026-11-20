import { Injectable } from "@angular/core";
import { HttpService } from "../../../core/http/http.service";
import { Result } from "@crossdyne/toolkit";
import { AssetUrlResponse } from "../models/dto/asset-urls.response";

@Injectable({
    providedIn: 'root'
})
export class AssetService extends HttpService {

    constructor() {
        super('api/v1/asset');
    }

    async getAllAsync() : Promise<Result<AssetUrlResponse[]>> {
        return await this.getAsync('');
    }
}