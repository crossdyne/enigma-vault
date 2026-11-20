import { inject, Injectable } from "@angular/core";
import { OverviewPayload } from "../models/domain/overview-payload";
import { CreditCard } from "../models/domain/credit-card";
import { StandardPassword } from "../models/domain/standard-password";
import { Server } from "../models/domain/server";
import { ApiKey } from "../models/domain/api-key";
import { VaultTypeEnum } from "../models/domain/vault-type.enum";
import { CryptoConstants } from "../../../core/constants/security.constants";
import { CryptoWorkerService } from "../../../core/services/crypto-worker.service";
import { ConnectionString } from "../models/domain/connection-string";
import { AsymmetricKey } from "../models/domain/asymmetric-key";
import { RecoveryKey } from "../models/domain/recovery-keys";
import { Email } from "../models/domain/email";
import { HardwareAccess } from "../models/domain/hardware-access";
import { ActivationKey } from "../models/domain/activation-key";

@Injectable({ providedIn: 'root' })
export class VaultCryptoService {
    private worker = inject(CryptoWorkerService);

    async decryptOverview(encrypted: string): Promise<OverviewPayload | null> {
        if (!encrypted) 
            return null;
        
        return this.worker.decrypt<OverviewPayload>(encrypted);
    }

    async decryptDetails(type: VaultTypeEnum, encrypted: string): Promise<StandardPassword | CreditCard | Server | ApiKey | ConnectionString | AsymmetricKey | RecoveryKey | null> {
        if (!encrypted) 
            return null;

        switch (type as number) {
            case VaultTypeEnum.Password:
                return await this.worker.decrypt<StandardPassword>(encrypted);
            case VaultTypeEnum.RecoveryKeys:
                return await this.worker.decrypt<RecoveryKey>(encrypted);
            case VaultTypeEnum.CreditCard:
                return await this.worker.decrypt<CreditCard>(encrypted);
            case VaultTypeEnum.Server:
                return await this.worker.decrypt<Server>(encrypted);
            case VaultTypeEnum.ApiKey:
                return await this.worker.decrypt<ApiKey>(encrypted);
            case VaultTypeEnum.ConnectionString:
                return await this.worker.decrypt<ConnectionString>(encrypted);
            case VaultTypeEnum.AsymmetricKey:
                return await this.worker.decrypt<AsymmetricKey>(encrypted);
            case VaultTypeEnum.Email:
                return await this.worker.decrypt<Email>(encrypted);
            case VaultTypeEnum.HardwareAccess: 
                return await this.worker.decrypt<HardwareAccess>(encrypted);
            case VaultTypeEnum.ActivationKey:
                return await this.worker.decrypt<ActivationKey>(encrypted);
            default:
                throw new Error(`Неизвестный тип: ${type}`);
        }
    }
    
    async encryptOverview(payload: OverviewPayload): Promise<{ encryptedOverView: string, cryptoVersion: number }> {
        const result = await this.worker.encrypt(payload, CryptoConstants.ACTUAL_CRYPTO_VERSION);
        return {
            encryptedOverView: result.encryptedData,
            cryptoVersion: result.cryptoVersion
        };
    }

    async encryptDetails(payload: any): Promise<{ encryptedDetails: string, cryptoVersion: number }> {
        const result = await this.worker.encrypt(payload, CryptoConstants.ACTUAL_CRYPTO_VERSION);
        return {
            encryptedDetails: result.encryptedData,
            cryptoVersion: result.cryptoVersion
        };
    }
}