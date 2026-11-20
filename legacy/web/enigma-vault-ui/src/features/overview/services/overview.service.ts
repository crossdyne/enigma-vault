import { Result } from "@crossdyne/toolkit";
import { HttpService } from "../../../core/http/http.service";
import { CountRecordsResponse } from "../models/count-records.response";
import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root'
})
export class OverviewService extends HttpService {

    constructor(){
        super('');
    }

    async recordsCount(): Promise<Result<CountRecordsResponse>> {
        return await this.getAsync<CountRecordsResponse>('api/v1/overview/records/count'); 
    }
}