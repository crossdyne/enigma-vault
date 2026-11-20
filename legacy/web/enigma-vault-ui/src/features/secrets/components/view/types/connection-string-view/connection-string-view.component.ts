import { Component, input } from "@angular/core";
import { CopyButton } from "../../../../../../shared/ui/copy-button/copy-button";
import { ConnectionString } from "../../../../models/domain/connection-string";

@Component({
    selector: 'connection-string-view',
    templateUrl: './connection-string-view.component.html',
    styleUrls: ['./connection-string-view.component.scss'],
    standalone: true,
    imports: [
        CopyButton
    ]
})
export class ConnectionStringViewComponent {
    data = input.required<ConnectionString>();
}