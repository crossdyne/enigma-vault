import { Component, input } from "@angular/core";
import { HardwareAccess } from "../../../../models/domain/hardware-access";
import { CopyButton } from "../../../../../../shared/ui/copy-button/copy-button";

@Component({
    selector: 'hardware-access-view',
    templateUrl: './hardware-access-view.component.html',
    styleUrls: ['./hardware-access-view.component.scss'],
    standalone: true,
    imports: [CopyButton]
})
export class HardwareAccessViewComponent {
    data = input.required<HardwareAccess>();
}