import { Injectable } from "@angular/core";
import { HttpService } from "../../../core/http/http.service";
import { Result } from "@crossdyne/toolkit";
import { EncryptedVaultResponse } from "../models/dto/encrypted-vault.response";
import { CreateVaultItemRequest } from "../models/dto/create-vault.request";
import { UpdateVaultItemRequest } from "../models/dto/update-vault-item.request";
import { UpdateTagsRequest } from "../models/dto/update-tags.request";
import { DateUpdateResponse } from "../models/dto/date-update.response";

@Injectable({
    providedIn: 'root'
})
export class VaultService extends HttpService {

    constructor() {
        super('api/v1/vault/')
    }

    async createAsync(request: CreateVaultItemRequest): Promise<Result<string>> {
        return await this.postAsync('', request);
    }

    async updateAsync(request: UpdateVaultItemRequest): Promise<Result<string>> {
        return await this.putAsync('', request);
    }

    async getAllAsync(): Promise<Result<EncryptedVaultResponse[]>> {
        return this.getAsync('');
    }

    async getByIdAsync(id: string): Promise<Result<EncryptedVaultResponse>> {
        return this.getAsync(`${id}`);
    }

    async moveToTrashAsync(id: string): Promise<Result<Date>> {
        return this.patchAsync(`trash/${id}`, null);
    }

    async restoreAllFromTrashAsync(): Promise<Result> {
        return this.patchAsync(`restore/all`, null);
    }

    async restoreFromTrashAsync(id: string): Promise<Result> {
        return this.patchAsync(`restore/${id}`, null);
    }

    async removeAsync(id: string): Promise<Result> {
        return await this.deleteAsync(`${id}`);
    }

    async emptyTrashAsync(): Promise<Result> {
        return await this.patchAsync('trash/empty', null);
    }

    async zipAsync(id: string): Promise<Result> {
        return this.patchAsync(`zip/${id}`, null);
    }

    async unZipAsync(id: string): Promise<Result> {
        return this.patchAsync(`unzip/${id}`, null);
    }

    async unZipAllAsync(): Promise<Result> {
        return this.patchAsync(`unzip/all`, null);
    }

    async changeIcon(vaultId: string, iconId: string): Promise<Result<DateUpdateResponse>> {
        return this.patchAsync(`change/${vaultId}/icon/${iconId}`, null);
    }

    async updateTagsAsync(id: string, request: UpdateTagsRequest): Promise<Result<DateUpdateResponse>> {
        return await this.patchAsync(`tags/${id}`, request)
    }

    async favorite(id: string): Promise<Result> {
        return await this.patchAsync(`favorite/${id}`, null);
    }

    async unFavorite(id: string): Promise<Result> {
        return await this.patchAsync(`unfavorite/${id}`, null);
    }
}