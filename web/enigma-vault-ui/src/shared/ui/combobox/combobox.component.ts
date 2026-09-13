import { Component, ElementRef, EventEmitter, HostListener, inject, Input, model, output, Output, signal } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
    selector: 'combobox',
    imports: [],
    templateUrl: './combobox.component.html',
    styleUrl: './combobox.component.scss',
    standalone: true,
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: ComboboxComponent,
            multi: true
        }
    ]
})
export class ComboboxComponent<T extends Record<string, any>> implements ControlValueAccessor {
    private elementRef = inject(ElementRef);

    @Input() items: T[] = [];
    @Input() placeholder = 'Выберите значение...';
    @Input() disabled = false;
    @Input() displayKey: keyof T = 'name' as keyof T;

    isOpen = signal(false);

    selectionChange = output<T | null>();
    selected = model<T | null>();

    private onChange: (value: T | null) => void = () => { };
    private onTouched: () => void = () => { };

    @HostListener('document:click', ['$event'])
    onDocumentClick(event: MouseEvent): void {
        if (!this.elementRef.nativeElement.contains(event.target) && this.isOpen()) {
            this.isOpen.set(false);
            this.onTouched();
        }
    }

    get displayValue(): string {
        const sel = this.selected();
        if (!sel) return this.placeholder;
        if (this.displayKey) {
            return String(sel[this.displayKey]);
        }
        return String(sel);
    }

    writeValue(value: T | null): void {
        this.selected.set(value);
    }

    registerOnChange(fn: (value: T | null) => void): void {
        this.onChange = fn;
    }

    registerOnTouched(fn: () => void): void {
        this.onTouched = fn;
    }

    setDisabledState(isDisabled: boolean): void {
        this.disabled = isDisabled;
    }

    selectItem(item: T | null): void {
        this.selected.set(item);
        this.onChange(item);
        this.selectionChange.emit(item);
        this.isOpen.set(false);
    }

    toggleDropdown(): void {
        if (!this.disabled) {
            this.isOpen.update(v => !v);
        }
    }

    clear(event: MouseEvent): void {
        event.stopPropagation();
        this.selected.set(null);
        this.onChange(null);
        this.selectionChange.emit(null);
    }
}