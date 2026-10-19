import { DIALOG_DATA, DialogRef } from "@angular/cdk/dialog";
import { CommonModule } from "@angular/common";
import { Component, computed, DestroyRef, inject, model, signal } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { VaultItemFormData } from "../../models/modal/vault-item-form.data";
import { VaultTypeEnum } from "../../models/domain/vault-type.enum";
import { FormVaultItemResult } from "../../models/modal/form-vault-item.result";
import { VaultItemDisplay } from "../../models/domain/vault-item-display";
import { PasswordComponent } from "./types/password/password.component";
import { CreditCardComponent } from "./types/credit-card/credit-card.component";
import { ServerComponent } from "./types/server/server.component";
import { ApiKeyFormComponent } from "./types/api-key/api-key.component";
import { ConnectionStringComponent } from "./types/connection-string/connection-string.component";
import { AsymmetricKeyComponent } from "./types/asymmetric-key/asymmetric-key.component";
import { OverviewPayload } from "../../models/domain/overview-payload";
import { StandardPassword } from "../../models/domain/standard-password";
import { Server } from "../../models/domain/server";
import { CreditCard } from "../../models/domain/credit-card";
import { ApiKey } from "../../models/domain/api-key";
import { ConnectionString } from "../../models/domain/connection-string";
import { AsymmetricKey } from "../../models/domain/asymmetric-key";
import { VaultItemCommon } from "../../models/modal/vault-item-common.modal";
import { RecoveryKeysFormComponent } from "./types/recovery-keys/recovery-keys.component";
import { RecoveryKeys } from "../../models/domain/recovery-keys";
import { Email } from "../../models/domain/email";
import { EmailCardComponent } from "./types/email/email-card.component";

@Component({
    selector: 'create-vault',
    templateUrl: './vault-item-form.component.html',
    styleUrls: ['./vault-item-form.component.scss'],
    standalone: true,
    imports: [
        CommonModule,
        ReactiveFormsModule,
        PasswordComponent,
        CreditCardComponent,
        ServerComponent,
        ApiKeyFormComponent,
        ConnectionStringComponent,
        AsymmetricKeyComponent,
        RecoveryKeysFormComponent,
        EmailCardComponent
    ]
})
export class VaultItemFormComponent {
    private dialogRef = inject(DialogRef<FormVaultItemResult>);
    private destroyRef = inject(DestroyRef); 
    data = inject(DIALOG_DATA) as VaultItemFormData;

    constructor() {
        if (this.data.mode === 'edit' && this.data.item) {
            this.isEdit.set(true);
            this.activeTab.set(this.data.item.type);

            this.overview.set({
                ServiceName: this.data.item.serviceName,
                Url: this.data.item.url,
                Note: this.data.item.note!,
            });

            if (this.data.decryptedDetails) {
                switch (this.data.item.type) {

                    case VaultTypeEnum.Password:
                        const sp = this.data.decryptedDetails as StandardPassword;
                        this.standardPassword.set({
                            Login: sp.Login, 
                            Password: sp.Password, 
                            Email: sp.Email, 
                            Phone: sp.Phone, 
                            SecretWord: sp.SecretWord, 
                            RecoveryKey: sp.RecoveryKey,
                        });
                        break;

                    case VaultTypeEnum.RecoveryKeys:
                        const recoveryKeys = this.data.decryptedDetails as RecoveryKeys;
                        this.recoveryKeys.set({
                            Application: recoveryKeys.Application,
                            DateCreation: recoveryKeys.DateCreation,
                            DateExpire: recoveryKeys.DateExpire,
                            Keys: recoveryKeys.Keys,
                        });
                        break;

                    case VaultTypeEnum.Server:
                        const s = this.data.decryptedDetails as Server;
                        this.server.set({
                            IpAddress: s.IpAddress,
                            Port: s.Port,
                            Domain: s.Domain,
                            Login: s.Login,
                            RootPassword: s.RootPassword,
                            SshKey: s.SshKey,
                        });
                        break;

                    case VaultTypeEnum.CreditCard:
                        const cc = this.data.decryptedDetails as CreditCard;
                        this.creditCard.set({
                            CardNumber: cc.CardNumber,
                            CardHolder: cc.CardHolder,
                            ExpireDate: cc.ExpireDate,
                            CvvCode: cc.CvvCode,
                            PinCode: cc.PinCode,
                            BankName: cc.BankName,
                            PaymentSystem: cc.PaymentSystem,
                        });
                        break;

                    case VaultTypeEnum.ApiKey:
                        const api = this.data.decryptedDetails as ApiKey;
                        this.apiKey.set({
                            Key: api.Key,
                            BaseUrl: api.BaseUrl,
                            ClientId: api.ClientId,
                            ClientSecret: api.ClientSecret,
                            ExpirationDate: api.ExpirationDate,
                            Environment: api.Environment,
                            Scope: api.Scope,
                        });
                        break;

                    case VaultTypeEnum.ConnectionString:
                        const cs = this.data.decryptedDetails as ConnectionString;
                        this.connectionString.set({
                            Value: cs.Value,
                            Application: cs.Application,
                        });
                        break;

                    case VaultTypeEnum.AsymmetricKey:
                        const ak = this.data.decryptedDetails as AsymmetricKey;
                        this.asymmetricKey.set({
                            PublicKey: ak.PublicKey,
                            PrivateKey: ak.PublicKey,
                            Application: ak.Application,
                            Algorithm: ak.Algorithm,
                            KeySize: ak.KeySize,
                            Passphrase: ak.Passphrase,
                            Format: ak.Format,
                            Fingerprint: ak.Fingerprint,
                            DateCreation: ak.DateCreation,
                            DateExpire: ak.DateExpire,
                        });
                        break;

                    case VaultTypeEnum.Email:
                        const email = this.data.decryptedDetails as Email;
                        this.email.set({
                            Login: email.Login,
                            Email: email.Email,
                            Password: email.Password,
                            SmtpServer: email.SmtpServer,
                            SmtpPort: email.SmtpPort,
                            EncryptionType: email.EncryptionType,
                        });
                        break;

                    default:
                        break;
                }
            }
        }

        this.destroyRef.onDestroy(() => {
            this.wipeAllSensitiveData();
        });
    }

    readonly vault = signal<VaultItemDisplay | null>(null);

    //#region Состояние формы

    activeTab = signal<VaultTypeEnum>(VaultTypeEnum.Password);
    readonly isEdit = signal(false);

    setActiveTab(tab: VaultTypeEnum) {
        if (this.isEdit()) {
            return;
        }

        this.activeTab.set(tab);
    }

    //#endregion

    //#region Заголовок формы

    VaultTypeEnum = VaultTypeEnum;

    vaultTypeEnToRu: Record<VaultTypeEnum, string> = {
        1: "Пароля",
        2: "Кредитной карты",
        3: "Доступа к серверу",
        4: "Апи ключа",
        5: "Строки подключения",
        6: "Ассиметричных ключей",
        7: "Ключей восстановления",
        8: "SMTP Сервера - Электронная почты"
    } 

    title = computed(() => this.vaultTypeEnToRu[this.activeTab()]);
    fullTitle = computed(() => `${this.isEdit() ? 'Редактирование' : 'Создание'} "${this.title()}"`);

    //#endregion
    
    //#region Управление данными

    overview = signal<OverviewPayload>({ ServiceName: '', Note: '', Url: '' });
    standardPassword = signal<StandardPassword>({ Login: '', Password: '', Email: '', Phone: '', SecretWord: '', RecoveryKey: '' });
    recoveryKeys = signal<RecoveryKeys>({ Application: '', DateCreation: '', DateExpire: '', Keys: [] });
    server = signal<Server>({ IpAddress: '', Port: '', Domain: '', Login: '', RootPassword: '', SshKey: '' });
    creditCard = signal<CreditCard>({ CardNumber: '', CardHolder: '',  ExpireDate: '', CvvCode: '', PinCode: '', BankName: '', PaymentSystem: '' });
    apiKey = signal<ApiKey>({ Key: '', BaseUrl: '', ClientId: '', ClientSecret: '', ExpirationDate: '', Environment: '', Scope: '' });
    connectionString = signal<ConnectionString>({ Value: '', Application: '' });
    asymmetricKey = signal<AsymmetricKey>({ PublicKey: '', PrivateKey: '', Application: '', Algorithm: '', KeySize: '', Passphrase: '', Format: '', Fingerprint: '', DateCreation: '', DateExpire: '' });
    email = signal<Email>({ Login: '', Email: '', Password: '', SmtpPort: '', SmtpServer: '', EncryptionType: '' });

    confirm() {
        let result: FormVaultItemResult;

        const common: VaultItemCommon = {
            name: this.overview().ServiceName,
            url: this.overview().Url,
            description: this.overview().Note
        };

        switch (this.activeTab()) {
            case VaultTypeEnum.Password:
                result = {
                    type: VaultTypeEnum.Password,
                    common,
                    details: {
                        Login: this.standardPassword().Login,
                        Password: this.standardPassword().Password,
                        Email: this.standardPassword().Email,
                        Phone: this.standardPassword().Phone,
                        SecretWord: this.standardPassword().SecretWord,
                        RecoveryKey: this.standardPassword().RecoveryKey
                    }
                }
                break;
            case VaultTypeEnum.RecoveryKeys:
                result = {
                    type: VaultTypeEnum.RecoveryKeys,
                    common,
                    details: {
                        Application: this.recoveryKeys().Application,
                        DateCreation: this.recoveryKeys().DateCreation,
                        DateExpire: this.recoveryKeys().DateExpire,
                        Keys: this.recoveryKeys().Keys,
                    }
                }
                break;
            case VaultTypeEnum.Server:
                result = {
                    type: VaultTypeEnum.Server,
                    common,
                    details: {
                        IpAddress: this.server().IpAddress,
                        Port: this.server().Port,
                        Domain: this.server().Domain,
                        Login: this.server().Login,
                        RootPassword: this.server().RootPassword,
                        SshKey: this.server().SshKey
                    }
                }
                break;
            case VaultTypeEnum.CreditCard:
                result = {
                    type: VaultTypeEnum.CreditCard,
                    common,
                    details: {
                        CardNumber: this.creditCard().CardNumber,
                        CardHolder: this.creditCard().CardHolder,
                        ExpireDate: this.creditCard().ExpireDate,
                        CvvCode: this.creditCard().CvvCode,
                        PinCode: this.creditCard().PinCode,
                        BankName: this.creditCard().BankName,
                        PaymentSystem: this.creditCard().PaymentSystem
                    }
                }
                break;
            case VaultTypeEnum.ApiKey:
                result = {
                    type: VaultTypeEnum.ApiKey,
                    common,
                    details: {
                        Key: this.apiKey().Key,
                        BaseUrl: this.apiKey().BaseUrl,
                        ClientId: this.apiKey().ClientId,
                        ClientSecret: this.apiKey().ClientSecret,
                        ExpirationDate: this.apiKey().ExpirationDate,
                        Environment: this.apiKey().Environment,
                        Scope: this.apiKey().Scope
                    }
                }
                break;
            case VaultTypeEnum.ConnectionString:
                result = {
                    type: VaultTypeEnum.ConnectionString,
                    common,
                    details: {
                        Value: this.connectionString().Value,
                        Application: this.connectionString().Application,
                    }
                }
                break;
            case VaultTypeEnum.AsymmetricKey:
                result = {
                    type: VaultTypeEnum.AsymmetricKey,
                    common,
                    details: {
                        PublicKey: this.asymmetricKey().PublicKey,
                        PrivateKey: this.asymmetricKey().PrivateKey,
                        Application: this.asymmetricKey().Application,
                        Algorithm: this.asymmetricKey().Algorithm,
                        KeySize: this.asymmetricKey().KeySize,
                        Passphrase: this.asymmetricKey().Passphrase,
                        Format: this.asymmetricKey().Format,
                        Fingerprint: this.asymmetricKey().Fingerprint,
                        DateCreation: this.asymmetricKey().DateCreation,
                        DateExpire: this.asymmetricKey().DateExpire,
                    }
                }
                break;
            case VaultTypeEnum.Email: 
                result = {
                    type: VaultTypeEnum.Email,
                    common,
                    details: {
                        Login: this.email().Login,
                        Email: this.email().Email,
                        Password: this.email().Password,
                        SmtpServer: this.email().SmtpServer,
                        SmtpPort: this.email().SmtpPort,
                        EncryptionType: this.email().EncryptionType,
                    }
                }
                break;
            default:
                return;
        }
                
        if (this.isEdit() && this.data.item?.id) {
            result = { ...result, id: this.data.item.id}
        }

        this.dialogRef.close(result);
        this.wipeAllSensitiveData(); 
    }

    private wipeAllSensitiveData(): void {
        this.overview.set({ ServiceName: '', Note: '', Url: '' });
        this.standardPassword.set({ Login: '', Password: '', Email: '', Phone: '', SecretWord: '', RecoveryKey: '' });
        this.recoveryKeys.set({ Application: '', DateCreation: '', DateExpire: '', Keys: [] });
        this.server.set({ IpAddress: '', Port: '', Domain: '', Login: '', RootPassword: '', SshKey: '' });
        this.creditCard.set({ CardNumber: '', CardHolder: '', ExpireDate: '', CvvCode: '', PinCode: '', BankName: '', PaymentSystem: '' });
        this.apiKey.set({ Key: '', BaseUrl: '', ClientId: '', ClientSecret: '', ExpirationDate: '', Environment: '', Scope: '' });
        this.connectionString.set({ Value: '', Application: '' });
        this.asymmetricKey.set({ PublicKey: '', PrivateKey: '', Application: '', Algorithm: '', KeySize: '', Passphrase: '', Format: '', Fingerprint: '', DateCreation: '', DateExpire: '' });
    }

    //#endregion

    //#region Управление модальным окном

    onCancel() {
        this.wipeAllSensitiveData();
        this.dialogRef.close();
    }

    //#endregion

}