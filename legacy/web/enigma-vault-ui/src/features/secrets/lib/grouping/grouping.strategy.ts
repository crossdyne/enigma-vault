import { VaultItemDisplay } from "../../models/domain/vault-item-display";
import { GroupingVaultsResult } from "../../types/grouping-vault.result";

export interface GroupingStrategy {
    group(vaults: VaultItemDisplay[], sorting: SortBy) : GroupingVaultsResult[];
}