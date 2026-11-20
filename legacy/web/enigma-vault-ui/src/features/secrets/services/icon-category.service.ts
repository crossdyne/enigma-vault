import { Injectable } from "@angular/core";
import { HttpService } from "../../../core/http/http.service";
import { Result } from "@crossdyne/toolkit";
import { IconCategoryResponse } from "../models/dto/icon-category.response";

@Injectable({
    providedIn: 'root'
})
export class IconCategoryService extends HttpService {

    constructor() {
        super('api/v1/icon/category');
    }

    async getAllAsync(): Promise<Result<IconCategoryResponse[]>> {
        return await this.getAsync('');
    }
}