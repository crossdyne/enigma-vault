import { Component, input } from "@angular/core";
import { StandardPassword } from "../../../../models/domain/standard-password";
import { CopyButton } from "../../../../../../shared/ui/copy-button/copy-button";

@Component({
    selector: 'standard-password-view',
    templateUrl: './password-view.component.html',
    styleUrls: ['./password-view.component.scss'],
    standalone: true,
    imports: [
        CopyButton
    ]
})
export class PasswordViewComponent {
    data = input.required<StandardPassword>();
}