import { Component, model } from "@angular/core";
import { ActivationKey } from "../../../../models/domain/activation-key";

@Component({
    selector: 'activation-key-form',
    templateUrl: './activation-key.component.html',
    styleUrls: ['./activation-key.component.scss'],
    standalone: true
})
export class ActivationKeyFormComponent {
    data = model.required<ActivationKey>();
}