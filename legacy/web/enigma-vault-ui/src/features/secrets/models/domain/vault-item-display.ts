import { IconUrl } from "../dto/icon-url";
import { TagResponse } from "../dto/tag.response";
import { VaultTypeEnum } from "./vault-type.enum";

export interface VaultItemDisplay {
    id: string; 
    serviceName: string;
    url: string;
    type: VaultTypeEnum;
    dateAdded: Date;
    dateUpdate: Date | null;
    deletedAt: number | null;
    isFavorite: boolean;
    isArchive: boolean; 
    isInTrash: boolean;
    encryptedOverview: string; 
    encryptedDetails: string, 
    tags: TagResponse[]; 
    icon: IconUrl | undefined;
    note: string | null;
}