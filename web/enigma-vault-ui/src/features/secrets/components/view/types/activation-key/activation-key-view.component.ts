import { Component, model } from "@angular/core";
import { ActivationKey } from "../../../../models/domain/activation-key";
import { CopyButton } from "../../../../../../shared/ui/copy-button/copy-button";

@Component({
    selector: 'activation-key-view',
    templateUrl: './activation-key-view.component.html',
    styleUrls: ['./activation-key-view.component.scss'],
    standalone: true,
    imports: [
        CopyButton
    ]
})
export class ActivationKeyViewComponent {
    data = model.required<ActivationKey>();
}