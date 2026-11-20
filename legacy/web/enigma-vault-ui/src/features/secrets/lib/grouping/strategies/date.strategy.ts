import { VaultItemDisplay } from "../../../models/domain/vault-item-display";
import { GroupingVaultsResult } from "../../../types/grouping-vault.result";
import { vaultsByServiceName } from "../../sorting/vaults-by-service-name.sort";
import { GroupingStrategy } from "../grouping.strategy";

export class DateGroupingStrategy implements GroupingStrategy {
    group(vaults: VaultItemDisplay[], sorting: SortBy): GroupingVaultsResult[] {
        const now = new Date();
        const today = new Date(now.getFullYear(), now.getMonth(), now.getDate());
        const todayTime = today.getTime();
                    
        const yesterday = new Date(today);
        yesterday.setDate(yesterday.getDate() - 1);
        const yesterdayTime = yesterday.getTime();
                    
        const weekAgo = new Date(today);
        weekAgo.setDate(weekAgo.getDate() - 7);
        const weekAgoTime = weekAgo.getTime();
        const groups: Record<string, VaultItemDisplay[]> = {
            'Сегодня': [],
            'Вчера': [],
            'На этой неделе': [],
            'В этом месяце': [],
            'Ранее': []
        };
        for (const vault of vaults) {
            const d = vault.dateAdded instanceof Date ? vault.dateAdded : new Date(vault.dateAdded);
            
            if (isNaN(d.getTime())) {
                groups['Ранее'].push(vault);
                continue;
            }
            const dStart = new Date(d.getFullYear(), d.getMonth(), d.getDate());
            const dTime = dStart.getTime();
            if (dTime === todayTime) {
                groups['Сегодня'].push(vault);
            } else if (dTime === yesterdayTime) {
                groups['Вчера'].push(vault);
            } else if (dTime >= weekAgoTime) {
                groups['На этой неделе'].push(vault);
            } else if (d.getMonth() === now.getMonth() && d.getFullYear() === now.getFullYear()) {
                groups['В этом месяце'].push(vault);
            } else {
                groups['Ранее'].push(vault);
            }
        }

        return Object.entries(groups).filter(([, vaults]) => vaults.length > 0).map(([title, vaults]) => ({ title, vaults: vaultsByServiceName(vaults, sorting) }));
    }
}