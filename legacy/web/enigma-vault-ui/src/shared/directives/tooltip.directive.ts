import { Directive, ElementRef, HostListener, Input, Renderer2, OnDestroy } from '@angular/core';

@Directive({
    selector: '[appTooltip]',
    standalone: true
})
export class TooltipDirective implements OnDestroy {
    @Input('appTooltip') text = '';
    private tooltip: HTMLElement | null = null;

    constructor(private el: ElementRef, private renderer: Renderer2) { }

    @HostListener('mouseenter')
    onMouseEnter() {
        if (!this.text || this.isMobile)
            return;

        this.tooltip = this.renderer.createElement('div');
        this.renderer.addClass(this.tooltip, 'app-tooltip');
        this.renderer.setProperty(this.tooltip, 'textContent', this.text);

        this.renderer.appendChild(document.body, this.tooltip);
        requestAnimationFrame(() => this.position());
    }

    @HostListener('mouseleave')
    onMouseLeave() {
        this.destroy();
    }

    ngOnDestroy() {
        this.destroy();
    }

    private position() {
        if (!this.tooltip)
            return;

        const rect = this.el.nativeElement.getBoundingClientRect();
        const tooltipRect = this.tooltip.getBoundingClientRect();
        const gap = 12;
        const padding = 8;

        const viewportWidth = window.innerWidth;
        const viewportHeight = window.innerHeight;

        const spaceLeft = rect.left - padding;
        const spaceRight = viewportWidth - rect.right - padding;
        const spaceTop = rect.top - padding;
        const spaceBottom = viewportHeight - rect.bottom - padding;

        let placement: 'top' | 'bottom' | 'left' | 'right' = 'top';
        let top = 0;
        let left = 0;

        const fitsHorizontally = (tooltipRect.width / 2) <= spaceLeft && (tooltipRect.width / 2) <= spaceRight;

        if (fitsHorizontally) {
            // Пытаемся открыть СВЕРХУ
            if (spaceTop >= tooltipRect.height + gap) {
                placement = 'top';
                top = rect.top - tooltipRect.height - gap;
                left = rect.left + (rect.width - tooltipRect.width) / 2;
            }
            // Если сверху не влезает, открываем СНИЗУ
            else if (spaceBottom >= tooltipRect.height + gap) {
                placement = 'bottom';
                top = rect.bottom + gap;
                left = rect.left + (rect.width - tooltipRect.width) / 2;
            }
            // Если и снизу не влезает, остаемся сверху, но сдвигаем
            else {
                placement = 'top';
                top = rect.top - tooltipRect.height - gap;
                left = rect.left + (rect.width - tooltipRect.width) / 2;

                if (left < padding)
                    left = padding;

                if (left + tooltipRect.width > viewportWidth - padding)
                    left = viewportWidth - tooltipRect.width - padding;
            }
        } else {
            // Если по ширине не влезает, пробуем открыть СПРАВА или СЛЕВА
            if (spaceRight >= tooltipRect.width + gap) {
                placement = 'right';
                left = rect.right + gap;
                top = rect.top + (rect.height - tooltipRect.height) / 2;
            } else if (spaceLeft >= tooltipRect.width + gap) {
                placement = 'left';
                left = rect.left - tooltipRect.width - gap;
                top = rect.top + (rect.height - tooltipRect.height) / 2;
            } else {

                if (spaceTop >= tooltipRect.height + gap) {
                    placement = 'top';
                    top = rect.top - tooltipRect.height - gap;
                } else {
                    placement = 'bottom';
                    top = rect.bottom + gap;
                }
                left = rect.left + (rect.width - tooltipRect.width) / 2;
                if (left < padding)
                    left = padding;

                if (left + tooltipRect.width > viewportWidth - padding)
                    left = viewportWidth - tooltipRect.width - padding;
            }
        }

        ['top', 'bottom', 'left', 'right'].forEach(p =>
            this.renderer.removeClass(this.tooltip, `app-tooltip--${p}`)
        );
        this.renderer.addClass(this.tooltip, `app-tooltip--${placement}`);

        let arrowOffset = '50%';
        if (placement === 'top' || placement === 'bottom') {
            const elementCenterX = rect.left + rect.width / 2;
            let offsetPx = elementCenterX - left;
            offsetPx = Math.max(12, Math.min(tooltipRect.width - 12, offsetPx));
            arrowOffset = `${offsetPx}px`;
        } else if (placement === 'left' || placement === 'right') {
            const elementCenterY = rect.top + rect.height / 2;
            let offsetPx = elementCenterY - top;
            offsetPx = Math.max(12, Math.min(tooltipRect.height - 12, offsetPx));
            arrowOffset = `${offsetPx}px`;
        }

        this.renderer.setStyle(this.tooltip, 'top', `${top}px`);
        this.renderer.setStyle(this.tooltip, 'left', `${left}px`);
        this.renderer.setStyle(this.tooltip, '--arrow-offset', arrowOffset);
    }

    private get isMobile(): boolean {
        if (typeof window === 'undefined')
            return false;

        return window.matchMedia('(hover: none)').matches || window.matchMedia('(max-width: 768px)').matches;
    }

    private destroy() {
        if (this.tooltip) {
            this.renderer.removeChild(document.body, this.tooltip);
            this.tooltip = null;
        }
    }
}