import { VaultItemDisplay } from "../../../models/domain/vault-item-display";
import { VaultTypeEnum } from "../../../models/domain/vault-type.enum";
import { GroupingVaultsResult } from "../../../types/grouping-vault.result";
import { vaultsByServiceName } from "../../sorting/vaults-by-service-name.sort";
import { vaultsGroupByTitle } from "../../sorting/vaults-group-by-title.sort";
import { GroupingStrategy } from "../grouping.strategy";

export class TypeGroupingStrategy implements GroupingStrategy {
    group(vaults: VaultItemDisplay[], sorting: SortBy): GroupingVaultsResult[] {
        const labels: Record<string, string> = {
            [VaultTypeEnum.Password]: 'Пароли',
            [VaultTypeEnum.ApiKey]: 'API ключи',
            [VaultTypeEnum.CreditCard]: 'Банковские карты',
            [VaultTypeEnum.Server]: 'Серверы',
            [VaultTypeEnum.ConnectionString]: 'Строки подключения',
            [VaultTypeEnum.AsymmetricKey]: 'Ассиметричные ключи',
            [VaultTypeEnum.RecoveryKeys]: 'Ключи восстановления',
            [VaultTypeEnum.Email]: 'SMTP & Электронная почта',
            [VaultTypeEnum.HardwareAccess]: 'Аппаратный доступ',
            [VaultTypeEnum.ActivationKey]: 'Ключ активации',
        };
        
        const groups = new Map<string, VaultItemDisplay[]>();
        for (const vault of vaults) {
            const key = labels[vault.type] ?? 'Другое';
            if (!groups.has(key)) 
                groups.set(key, []);
            
            groups.get(key)!.push(vault);
        }

        return vaultsGroupByTitle(
            Array.from(groups.entries()).map(([title, vaults]) => ({ title, vaults: vaultsByServiceName(vaults, sorting)})), 
            sorting);
    }
}