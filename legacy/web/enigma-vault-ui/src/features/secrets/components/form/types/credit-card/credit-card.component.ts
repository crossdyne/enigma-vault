import { Component, model } from "@angular/core";
import { CreditCard } from "../../../../models/domain/credit-card";

@Component({
    selector: 'credit-card-form',
    templateUrl: './credit-card.component.html',
    styleUrls: ['./credit-card.component.scss'],
    standalone: true,
})
export class CreditCardComponent {
    data = model.required<CreditCard>({});
}