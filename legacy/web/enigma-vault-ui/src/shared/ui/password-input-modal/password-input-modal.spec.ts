import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PasswordInputModalComponent } from './password-input-modal';

describe('PasswordInputModal', () => {
    let component: PasswordInputModalComponent;
    let fixture: ComponentFixture<PasswordInputModalComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [PasswordInputModalComponent],
        }).compileComponents();

        fixture = TestBed.createComponent(PasswordInputModalComponent);
        component = fixture.componentInstance;
        await fixture.whenStable();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
