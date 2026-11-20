import { Component, input } from "@angular/core";
import { ApiKey } from "../../../../models/domain/api-key";
import { CopyButton } from "../../../../../../shared/ui/copy-button/copy-button";

@Component({
    selector: 'api-key-view',
    templateUrl: './api-key-view.component.html',
    styleUrls: ['./api-key-view.component.scss'],
    standalone: true,
    imports: [
        CopyButton
    ],
})
export class ApiKeyViewComponent {
    data = input.required<ApiKey>();
}