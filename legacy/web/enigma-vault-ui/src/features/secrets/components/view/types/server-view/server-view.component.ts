import { Component, input } from "@angular/core";
import { CopyButton } from "../../../../../../shared/ui/copy-button/copy-button";
import { Server } from "../../../../models/domain/server";

@Component({
    selector: 'server-view',
    templateUrl: './server-view.component.html',
    styleUrls: ['./server-view.component.scss'],
    standalone: true,
    imports: [
        CopyButton
    ]
})
export class ServerViewComponent {
    data = input.required<Server>();
}