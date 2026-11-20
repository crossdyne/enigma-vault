import { Component, inject, signal } from '@angular/core';
import { CryptoHttpService } from '../../../core/services/crypto-http.service';
import { CryptoWorkerService } from '../../../core/services/crypto-worker.service';
import { Result } from '@crossdyne/toolkit';
import { DekResponse } from '../../../core/contracts/crypto/dek.response';
import { DialogRef } from '@angular/cdk/dialog';
import { PasswordInputModalResult } from './models/password-input-modal.result';

@Component({
    selector: 'app-password-input-modal',
    imports: [],
    templateUrl: './password-input-modal.html',
    styleUrl: './password-input-modal.scss',
})
export class PasswordInputModalComponent {
    private cryptoHttp = inject(CryptoHttpService);
    private cryptoWorker = inject(CryptoWorkerService);
    private dialogRef = inject(DialogRef<PasswordInputModalResult>);

    errors = signal<string>('');
    password = signal<string>('');

    async submit() {
        this.errors.set('');

        if (!this.password()){
            this.errors.set('Вы не ввели пароль');
            return;
        }
        
        const result: Result<DekResponse> = await this.cryptoHttp.getDekAsync();
        
        if (result.isFailure) {
            this.errors.set(result.stringMessage);
            return;
        }

        const dekResponse = result.value;

        try {
            await this.cryptoWorker.init(
                dekResponse.login,
                this.password(),
                dekResponse.clientSalt,
                dekResponse.encryptedDek,
                dekResponse.cryptoVersion as any
            );
        } catch (error) {
            this.errors.set('Вы ввели не верный пароль');
            console.error(error);
            return;
        }

        this.password.set('');

        this.dialogRef.close({
            isCorrectPassword: true
        });
    }

    onCancel() {
        this.dialogRef.close({
            isCorrectPassword: false
        });
    }
}
