import { Component, input } from "@angular/core";
import { CopyButton } from "../../../../../../shared/ui/copy-button/copy-button";
import { CreditCard } from "../../../../models/domain/credit-card";

@Component({
    selector: 'credit-card-view',
    templateUrl: './credit-card-view.component.html',
    styleUrls: ['./credit-card-view.component.scss'],
    standalone: true,
    imports: [
        CopyButton
    ]
})
export class CreditCardViewComponent {
    data = input.required<CreditCard>();
}