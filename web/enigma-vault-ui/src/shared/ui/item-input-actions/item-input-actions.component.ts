import { Component, effect, input, output, signal } from '@angular/core';

@Component({
    selector: 'item-input-actions',
    imports: [],
    templateUrl: './item-input-actions.component.html',
    styleUrl: './item-input-actions.component.scss',
})
export class ItemInputActionsComponent<T> {
    editing = input<T | null>(null);

    getValue = input.required<(item: T) => string>();
    setValue = input.required<(item: T, value: string) => T>();

    createPlaceholder = input<string>('Введите название...');
    editPlaceholder = input<string>('Редактирование...');

    value = signal<string>('');

    cancelEdit = output<void>();
    update = output<T>();
    create = output<string>();

    constructor() {
        effect(() => {
            const item = this.editing();
            const getter = this.getValue();
            this.value.set(item ? getter(item) : '');
        });
    }

    onAdd() {
        const trimmed = this.value().trim();

        if (trimmed) {
            this.create.emit(trimmed);
            this.value.set('');
        }
    }

    onSave() {
        const item = this.editing();
        const setter = this.setValue();

        if (item) {
            this.update.emit(setter(item, this.value()));
        }
    }

    onCancel() {
        this.cancelEdit.emit();
    }
}