import { Component, model } from "@angular/core";
import { AsymmetricKey } from "../../../../models/domain/asymmetric-key";

@Component({
    selector: 'asymmetric-key-form',
    templateUrl: './asymmetric-key.component.html',
    styleUrls: ['./asymmetric-key.component.scss'],
    standalone: true,
})
export class AsymmetricKeyComponent {
    data = model.required<AsymmetricKey>({});
}