import { Component, input, Input, signal } from '@angular/core';

@Component({
    selector: 'copy-button',
    imports: [],
    templateUrl: './copy-button.html',
    styleUrl: './copy-button.scss',
})
export class CopyButton {
    value = input.required<string | null | undefined>();
    copied = signal(false);

    async copy() {
        if (!this.value) return;

        try {
            await navigator.clipboard.writeText(this.value()!);
            this.copied.set(true);
            setTimeout(() => this.copied.set(false), 2000);
        } catch (err) {
            console.error('Ошибка копирования в буфер обмена:', err);
        }
    }
}
