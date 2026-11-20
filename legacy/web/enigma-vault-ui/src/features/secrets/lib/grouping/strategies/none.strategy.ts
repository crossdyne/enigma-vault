import { VaultItemDisplay } from "../../../models/domain/vault-item-display";
import { GroupingVaultsResult } from "../../../types/grouping-vault.result";
import { GroupingStrategy } from "../grouping.strategy";

export class NoneGroupingStrategy implements GroupingStrategy {
    group(vaults: VaultItemDisplay[]): GroupingVaultsResult[] {
        const result: GroupingVaultsResult[] = [{ title: 'Все записи', vaults}];
        return result;
    }
}