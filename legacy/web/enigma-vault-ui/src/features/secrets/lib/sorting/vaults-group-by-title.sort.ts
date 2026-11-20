import { GroupingVaultsResult } from "../../types/grouping-vault.result";


export const vaultsGroupByTitle = (grouped: GroupingVaultsResult[], sorting: SortBy) => {
    if (sorting === 'none')
        return grouped;

    return grouped.sort((a, b) => {
        const titleA = a.title;
        const titleB = b.title;
        return sorting === 'ascending' ? titleA.localeCompare(titleB) : titleB.localeCompare(titleA);
    });
};