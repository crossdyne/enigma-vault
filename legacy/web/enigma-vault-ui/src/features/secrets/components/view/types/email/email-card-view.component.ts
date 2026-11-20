import { Component, input } from "@angular/core";
import { CopyButton } from "../../../../../../shared/ui/copy-button/copy-button";
import { Email } from "../../../../models/domain/email";

@Component({
    selector: 'email-card-view',
    templateUrl: './email-card-view.component.html',
    styleUrls: ['./email-card-view.component.scss'],
    standalone: true,
    imports: [
        CopyButton
    ]
})
export class EmailCardViewComponent {
    data = input.required<Email>();
}