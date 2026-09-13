import { Routes } from '@angular/router';
import { MainLayoutComponent } from '../core/layouts/main/main-layout.component';
import { PasswordsPageComponent } from '../features/secrets/pages/passwords/passwords-page.component';
import { OverviewPageComponent } from '../features/overview/pages/overview/overview-page.component';

export const routes: Routes = [
    { path: '', redirectTo: '/overview', pathMatch: 'full' },
    { 
        path: '',
        loadComponent: () => MainLayoutComponent,
        children: [
            { path: 'overview', loadComponent: () => OverviewPageComponent },
            { path: 'passwords', loadComponent: () => PasswordsPageComponent },
        ]
    }
];
