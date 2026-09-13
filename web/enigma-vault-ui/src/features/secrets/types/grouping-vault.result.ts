import { VaultItemDisplay } from "../models/domain/vault-item-display";

export interface GroupingVaultsResult {
    title: string;
    vaults: VaultItemDisplay[];
}