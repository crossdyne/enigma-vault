import { Component, model } from "@angular/core";
import { HardwareAccess } from "../../../../models/domain/hardware-access";

@Component({
    selector: 'hardware-access-form',
    templateUrl: './hardware-access.component.html',
    styleUrls: ['./hardware-access.component.scss'],
    standalone: true
})
export class HardwareAccessComponent {
    data = model.required<HardwareAccess>({});
}