import { Component, signal, ViewEncapsulation } from "@angular/core";
import { RouterLink, RouterOutlet } from "@angular/router";
import { environment } from "../../../environments/environment";

@Component({
    selector: 'app-main-layout',
    templateUrl: './main-layout.component.html',
    styleUrl: './main-layout.component.scss',
    standalone: true,
    encapsulation: ViewEncapsulation.None,
    imports: [
        RouterOutlet,
        RouterLink
    ]
})
export class MainLayoutComponent{
    accountUrl = signal<string>(environment.accountUrl);
    
    async logout() {
        const currentUrl = encodeURIComponent(window.location.href);
        window.location.href = `${environment.returnAuthUrlBase}?logout=true&returnUrl=${currentUrl}`;
    } 
}