import { Component, input } from "@angular/core";
import { CopyButton } from "../../../../../../shared/ui/copy-button/copy-button";
import { AsymmetricKey } from "../../../../models/domain/asymmetric-key";

@Component({
    selector: 'asymmetric-key-view',
    templateUrl: './asymmetric-key-view.component.html',
    styleUrls: ['./asymmetric-key-view.component.scss'],
    standalone: true,
    imports: [
        CopyButton
    ],
})
export class AsymmetricKeyViewComponent {
    data = input.required<AsymmetricKey>();
}