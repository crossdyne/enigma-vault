import { Component, computed, DestroyRef, inject, signal } from "@angular/core";
import { VaultTypeEnum } from "../../models/domain/vault-type.enum";
import { DIALOG_DATA, DialogRef } from "@angular/cdk/dialog";
import { OverviewPayload } from "../../models/domain/overview-payload";
import { VaultItemView } from "../../models/modal/vault-item-view";
import { CopyButton } from "../../../../shared/ui/copy-button/copy-button";
import { PasswordViewComponent } from "./types/password-view/password-view.component";
import { CreditCardViewComponent } from "./types/credit-card-view/credit-card-view.component";
import { ServerViewComponent } from "./types/server-view/server-view.component";
import { ApiKeyViewComponent } from "./types/api-key-view/api-key-view.component";
import { ConnectionStringViewComponent } from "./types/connection-string-view/connection-string-view.component";
import { AsymmetricKeyViewComponent } from "./types/asymmetric-key-view/asymmetric-key-view.component";
import { RecoveryKeysViewComponent } from "./types/recovery-keys/recovery-keys-view.component";
import { EmailCardViewComponent } from "./types/email/email-card-view.component";
import { HardwareAccessViewComponent } from "./types/hardware-access/hardware-access-view.component";

@Component({
    selector: 'vault-item-view',
    templateUrl: './vault-item-view.component.html',
    styleUrls: ['./vault-item-view.component.scss'],
    standalone: true,
    imports: [
        CopyButton,
        PasswordViewComponent,
        CreditCardViewComponent,
        CreditCardViewComponent,
        ServerViewComponent,
        ApiKeyViewComponent,
        ConnectionStringViewComponent,
        AsymmetricKeyViewComponent,
        RecoveryKeysViewComponent,
        EmailCardViewComponent,
        HardwareAccessViewComponent
    ]
})
export class VaultItemViewComponent {
    private dialogRef = inject(DialogRef);
    private destroyRef = inject(DestroyRef); 

    vaultTypeEnToRu: Record<VaultTypeEnum, string> = {
        1: "Пароль",
        2: "Кредитная карта",
        3: "Доступ к серверу",
        4: "Апи ключ",
        5: "Строки подключения",
        6: "Ассиметричный ключ",
        7: "Ключи восстановления",
        8: "SMTP - Электронная почта",
        9: "Аппаратного доступа",
    } 

    data = inject(DIALOG_DATA) as VaultItemView;
    VaultTypeEnum = VaultTypeEnum;
    
    type = signal<VaultTypeEnum>(this.data.type);
    typeRu = computed(() => this.vaultTypeEnToRu[this.type()] || 'Неизвестно');
    overview = signal<OverviewPayload>(this.data.overview);
    decryptedDetails = signal<any>(this.data.decryptedDetails);

    constructor() {
        this.type.set(this.data.type)
        this.overview.set(this.data.overview);
        this.decryptedDetails.set(this.data.decryptedDetails)

        this.destroyRef.onDestroy(() => {
            this.wipeDecryptedData();
        });
    }

    private wipeDecryptedData() {
        this.decryptedDetails.set(null);
    }

    onCancel(): void {
        this.wipeDecryptedData();
        this.dialogRef.close();
    }
}