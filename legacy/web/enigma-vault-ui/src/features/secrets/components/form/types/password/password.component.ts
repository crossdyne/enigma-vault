import { Component, model } from "@angular/core";
import { StandardPassword } from "../../../../models/domain/standard-password";

@Component({
    selector: 'password-form',
    templateUrl: './password.component.html',
    styleUrls: ['./password.component.scss'],
    standalone: true,
})
export class PasswordComponent {
    data = model.required<StandardPassword>({});
}