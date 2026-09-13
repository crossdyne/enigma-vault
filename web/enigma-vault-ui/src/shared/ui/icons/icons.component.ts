import { Component, computed, effect, inject, signal } from '@angular/core';
import { AssetUrlResponse } from '../../../features/secrets/models/dto/asset-urls.response';
import { IconCategoryResponse } from '../../../features/secrets/models/dto/icon-category.response';
import { TooltipDirective } from '../../directives/tooltip.directive';
import { DIALOG_DATA, DialogRef } from '@angular/cdk/dialog';
import { ChangeIconData } from './modal/change-icon.data';

@Component({
    selector: 'icons',
    templateUrl: './icons.component.html',
    styleUrl: './icons.component.scss',
    standalone: true,
    imports: [
        TooltipDirective
    ]
})
export class IconsComponent {

    constructor() {
        effect(() => {
            const icon = this.selectedIcon();

            if (!icon)
                return;

            this.dialogRef.close(icon!);
        });
    }

    private dialogRef = inject(DialogRef<AssetUrlResponse>);
    data = inject(DIALOG_DATA) as ChangeIconData;

    serviceName = signal<string>(this.data.serviceName);

    selectedIcon = signal<AssetUrlResponse | null>(null);

    icons = signal<AssetUrlResponse[]>(this.data.icons);
    categories = signal<IconCategoryResponse[]>(this.data.categories);
    groupedIcons = computed(() => {
        const icons = this.icons();
        const categories = this.categories();

        const categoryMap = new Map<string, string>();
        for (const cat of categories) {
            categoryMap.set(cat.categoryId, cat.name);
        }

        const groups = new Map<string, AssetUrlResponse[]>();
        for (const icon of icons) {
            const categoryName = categoryMap.get(icon.categoryId) || 'Без категории';

            if (!groups.has(categoryName))
                groups.set(categoryName, []);

            groups.get(categoryName)!.push(icon);
        }

        return Array.from(groups.entries())
            .sort(([titleA], [titleB]) => titleA.localeCompare(titleB, 'ru'))
            .map(([title, groupIcons]) => {
                const sortedIcons = [...groupIcons].sort((a, b) => a.assetName.localeCompare(b.assetName, 'ru'));

                return { title, icons: sortedIcons };
            })
    });

    navigateToCreateAssets() {
        window.open('https://assets.crossdyne.com/', '_blank')
    }

    onCancel() {
        this.dialogRef.close();
    }
}