import { Injectable } from "@angular/core";
import { HttpService } from "../../../core/http/http.service";
import { TagResponse } from "../models/dto/tag.response";
import { Result } from "@crossdyne/toolkit";
import { CreateTagRequest } from "../models/dto/create-tag.request";
import { UpdateTagRequest } from "../models/dto/update-tag.request";
import { CreateTagResponse } from "../models/dto/create-tag.response";

@Injectable({
    providedIn: 'root'
})
export class TagService extends HttpService {
    constructor(){
        super('api/v1/tag/')
    }

    async createAsync(request: CreateTagRequest): Promise<Result<CreateTagResponse>> {
        return await this.postAsync<CreateTagResponse>('', request);
    }

    async updateAsync(request: UpdateTagRequest): Promise<Result> {
        return await this.patchAsync('', request);
    }

    async removeAsync(id: string): Promise<Result> {
        return await this.deleteAsync(`${id}`);
    }

    async getAllAsync(): Promise<Result<TagResponse[]>> {
        return this.getAsync('');
    }
}