import { Component, model } from "@angular/core";
import { Server } from "../../../../models/domain/server";

@Component({
    selector: 'server-form',
    templateUrl: './server.component.html',
    styleUrls: ['./server.component.scss'],
    standalone: true,
})
export class ServerComponent {
    data = model.required<Server>({});
}