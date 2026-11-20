import { Component, model } from "@angular/core";
import { ConnectionString } from "../../../../models/domain/connection-string";

@Component({
    selector: 'connection-string-form',
    templateUrl: './connection-string.component.html',
    styleUrls: ['./connection-string.component.scss'],
    standalone: true,
})
export class ConnectionStringComponent {
    data = model.required<ConnectionString>({});
}