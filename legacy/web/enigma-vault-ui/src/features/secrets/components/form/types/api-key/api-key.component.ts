import { Component, model } from "@angular/core";
import { ApiKey } from "../../../../models/domain/api-key";

@Component({
    selector: 'api-key-form',
    templateUrl: 'api-key.component.html',
    styleUrls: ['./api-key.component.scss'],
    standalone: true
})
export class ApiKeyFormComponent {
    data = model.required<ApiKey>();
}