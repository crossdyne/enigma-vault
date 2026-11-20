import { Directive, ElementRef, OnDestroy, AfterViewInit } from '@angular/core';

@Directive({
    selector: '[appTagsOverflow]',
    standalone: true
})
export class TagsOverflowDirective implements AfterViewInit, OnDestroy {
    private resizeObserver?: ResizeObserver;
    private mutationObserver?: MutationObserver;

    constructor(private el: ElementRef<HTMLElement>) { }

    ngAfterViewInit(): void {
        const element = this.el.nativeElement;

        this.resizeObserver = new ResizeObserver(() => this.recalculate());
        this.resizeObserver.observe(element);

        this.mutationObserver = new MutationObserver(() => this.recalculate());
        this.mutationObserver.observe(element, { childList: true });

        requestAnimationFrame(() => this.recalculate());
    }

    private recalculate(): void {
        const container = this.el.nativeElement;
        const tags = Array.from(container.querySelectorAll<HTMLElement>('.tag'));
        const ellipsis = container.querySelector<HTMLElement>('.tags-ellipsis');

        if (!ellipsis || tags.length === 0) {
            if (ellipsis)
                ellipsis.style.display = 'none';

            return;
        }

        const containerWidth = container.clientWidth;
        const style = getComputedStyle(container);
        const gap = parseFloat(style.gap) || parseFloat(style.columnGap) || 0;

        tags.forEach(tag => tag.style.display = '');
        ellipsis.style.display = 'none';

        const tagWidths: number[] = [];
        let totalTagsWidth = 0;

        for (let i = 0; i < tags.length; i++) {
            const w = tags[i].offsetWidth;
            tagWidths.push(w);
            totalTagsWidth += w + (i > 0 ? gap : 0);
        }

        if (totalTagsWidth <= containerWidth) {
            ellipsis.style.display = 'none';
            return;
        }

        ellipsis.style.display = 'inline-block';
        const ellipsisWidth = ellipsis.offsetWidth;

        let currentWidth = 0;
        let visibleCount = 0;

        for (let i = 0; i < tags.length; i++) {
            const addedWidth = tagWidths[i] + (i > 0 ? gap : 0);
            const neededWidth = currentWidth + addedWidth + gap + ellipsisWidth;

            if (neededWidth <= containerWidth) {
                currentWidth += addedWidth;
                visibleCount++;
            } else {
                break;
            }
        }

        tags.forEach((tag, index) => {
            tag.style.display = index < visibleCount ? '' : 'none';
        });
    }

    ngOnDestroy(): void {
        this.resizeObserver?.disconnect();
        this.mutationObserver?.disconnect();
    }
}