import { Component, computed, inject, resource, signal } from "@angular/core";
import { Router } from "@angular/router";
import { CountRecordsResponse } from "../../models/count-records.response";
import { OverviewService } from "../../services/overview.service";
import { Result } from "@crossdyne/toolkit";

@Component({
    selector: 'overview-page',
    templateUrl: './overview-page.component.html',
    styleUrls: ['./overview-page.component.scss'],
    standalone: true
})
export class OverviewPageComponent {
    private router = inject(Router);
    private overviewService = inject(OverviewService);

    allRecordsCount = computed(() =>{
        const passwordCount = this.recordsCountResource.value()?.passwordsCount;

        return passwordCount;
    });

    recordsCountResource = resource({
        loader: async () => {
            const result: Result<CountRecordsResponse> = await this.overviewService.recordsCount();

            if (result.isFailure){
                console.error(result.stringMessage);
                return null;
            }

            return result.value;
        }
    });

    openPasswordsPage() {
        this.router.navigate(['/passwords']);
    }
}