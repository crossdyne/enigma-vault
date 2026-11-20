import { VaultItemDisplay } from "../../models/domain/vault-item-display";

export const vaultsByServiceName = (items: VaultItemDisplay[], sorting: SortBy) => {
    if (sorting === 'none')
        return items;

    return [...items].sort((a, b) => {
        const nameA = a.serviceName.toLowerCase();
        const nameB = b.serviceName.toLowerCase();
        return sorting === 'ascending' ? nameA.localeCompare(nameB) : nameB.localeCompare(nameA);
    });
};