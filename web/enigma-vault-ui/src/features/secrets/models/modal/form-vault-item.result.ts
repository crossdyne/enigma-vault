import { ApiKey } from "../domain/api-key";
import { AsymmetricKey } from "../domain/asymmetric-key";
import { ConnectionString } from "../domain/connection-string";
import { CreditCard } from "../domain/credit-card";
import { RecoveryKeys } from "../domain/recovery-keys";
import { Server } from "../domain/server";
import { StandardPassword } from "../domain/standard-password";
import { VaultTypeEnum } from "../domain/vault-type.enum";
import { VaultItemCommon } from "./vault-item-common.modal";

export interface FormVaultItemBase {
    id?: string;
    type: VaultTypeEnum;
    common: VaultItemCommon;
}

export type FormVaultItemResult =
    | (FormVaultItemBase & { type: VaultTypeEnum.Password; details: StandardPassword })
    | (FormVaultItemBase & { type: VaultTypeEnum.CreditCard; details: CreditCard })
    | (FormVaultItemBase & { type: VaultTypeEnum.Server; details: Server })
    | (FormVaultItemBase & { type: VaultTypeEnum.ApiKey; details: ApiKey })
    | (FormVaultItemBase & { type: VaultTypeEnum.ConnectionString; details: ConnectionString })
    | (FormVaultItemBase & { type: VaultTypeEnum.AsymmetricKey; details: AsymmetricKey })
    | (FormVaultItemBase & { type: VaultTypeEnum.RecoveryKeys; details: RecoveryKeys });