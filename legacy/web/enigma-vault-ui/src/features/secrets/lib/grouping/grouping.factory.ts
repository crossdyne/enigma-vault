import { GroupingStrategy } from "./grouping.strategy";
import { AlphabetGroupingStrategy } from "./strategies/alphabet.strategy";
import { DateGroupingStrategy } from "./strategies/date.strategy";
import { FavoritesGroupingStrategy } from "./strategies/favorite.strategy";
import { HistoryGroupingStrategy } from "./strategies/history.strategy";
import { NoneGroupingStrategy } from "./strategies/none.strategy";
import { TagsGroupingStrategy } from "./strategies/tags.strategy";
import { TypeGroupingStrategy } from "./strategies/type.strategy";

export class GroupingFactory {
    private static strategies = new Map<GroupBy, new () => GroupingStrategy>([
        ['none', NoneGroupingStrategy],
        ['alphabet',  AlphabetGroupingStrategy],
        ['type', TypeGroupingStrategy],
        ['date', DateGroupingStrategy],
        ['history', HistoryGroupingStrategy],
        ['tags', TagsGroupingStrategy],
        ['favorite', FavoritesGroupingStrategy]
    ]);

    static create(grouping: GroupBy) : GroupingStrategy {
        const impl = this.strategies.get(grouping);

        if (!impl)
            throw Error(`Стратегии для ${grouping} не существует.`);

        return new impl();
    }
}