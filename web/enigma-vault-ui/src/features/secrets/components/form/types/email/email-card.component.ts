import { Component, model } from "@angular/core";
import { Email } from "../../../../models/domain/email";

@Component({    
    selector: 'email-card-form',
    templateUrl: './email-card.component.html',
    styleUrls: ['./email-card.component.scss'],
    standalone: true,
})
export class EmailCardComponent {
    data = model.required<Email>({});
}