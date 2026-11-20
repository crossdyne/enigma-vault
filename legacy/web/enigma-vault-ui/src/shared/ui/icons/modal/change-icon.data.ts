import { AssetUrlResponse } from "../../../../features/secrets/models/dto/asset-urls.response";
import { IconCategoryResponse } from "../../../../features/secrets/models/dto/icon-category.response";


export interface ChangeIconData {
    icons: AssetUrlResponse[];
    categories: IconCategoryResponse[];
    serviceName: string;
    reloadIcons: () => Promise<{ icons: AssetUrlResponse[], categories: IconCategoryResponse[] }>;
}