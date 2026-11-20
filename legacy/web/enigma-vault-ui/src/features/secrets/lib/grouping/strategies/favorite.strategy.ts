import { VaultItemDisplay } from "../../../models/domain/vault-item-display";
import { GroupingVaultsResult } from "../../../types/grouping-vault.result";
import { vaultsByServiceName } from "../../sorting/vaults-by-service-name.sort";
import { GroupingStrategy } from "../grouping.strategy";

export class FavoritesGroupingStrategy implements GroupingStrategy {
    group(vaults: VaultItemDisplay[], sorting: SortBy): GroupingVaultsResult[] {
        const favorite = 'В избранном';
        const notFavorite = 'Не в избранном';

        const groups = new Map<string, VaultItemDisplay[]>();

        const favoritesVaults = vaults.filter(v => v.isFavorite);
        const notFavoritesVaults = vaults.filter(v => !v.isFavorite);

        if (favoritesVaults.length > 0)
            groups.set(favorite, favoritesVaults);

        groups.set(notFavorite, notFavoritesVaults);

        return Array.from(groups.entries())
            .sort(([a], [b]) => a.localeCompare(b))
            .map(([title, vaults]) => ({ title, vaults: vaultsByServiceName(vaults, sorting) }));
    }
}