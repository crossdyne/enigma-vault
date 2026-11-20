import { Component, model } from "@angular/core";
import { CopyButton } from "../../../../../../shared/ui/copy-button/copy-button";
import { RecoveryKeys } from "../../../../models/domain/recovery-keys";

@Component({
    selector: 'recovery-keys-view',
    templateUrl: './recovery-keys-view.component.html',
    styleUrls: ['./recovery-keys-view.component.scss'],
    standalone: true,
    imports: [
        CopyButton
    ]
})
export class RecoveryKeysViewComponent {
    data = model.required<RecoveryKeys>({});
}