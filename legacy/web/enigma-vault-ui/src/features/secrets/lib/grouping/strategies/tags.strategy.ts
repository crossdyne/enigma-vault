import { VaultItemDisplay } from "../../../models/domain/vault-item-display";
import { GroupingVaultsResult } from "../../../types/grouping-vault.result";
import { vaultsByServiceName } from "../../sorting/vaults-by-service-name.sort";
import { vaultsGroupByTitle } from "../../sorting/vaults-group-by-title.sort";
import { GroupingStrategy } from "../grouping.strategy";

export class TagsGroupingStrategy implements GroupingStrategy {
    group(vaults: VaultItemDisplay[], sorting: SortBy): GroupingVaultsResult[] {
        let result: GroupingVaultsResult[];

        const groups = new Map<string, VaultItemDisplay[]>();
        const untagged: VaultItemDisplay[] = [];

        for (const vault of vaults) {
            if (!vault.tags || vault.tags.length === 0) {
                untagged.push(vault);
                continue;
            }
            for (const tag of vault.tags) {
                const name = tag.name;
                if (!groups.has(name)) 
                    groups.set(name, []);

                groups.get(name)!.push(vault);
            }
        }

        result = vaultsGroupByTitle(
            Array.from(groups.entries()).sort(([a], [b]) => a.localeCompare(b)).map(([title, vaults]) => ({ title, vaults: vaultsByServiceName(vaults, sorting) })), 
            sorting);

        if (untagged.length > 0) {
            result.push({ title: 'Без тегов', vaults: vaultsByServiceName(untagged, sorting) });
        }

        return result;
    }
}