export interface EncryptedVaultResponse {
    id: string; 
    type: number;
    dateAdded: Date;
    dateUpdate: Date | null;
    deletedAt: Date | null;
    isFavorite: boolean;
    isArchive: boolean; 
    isInTrash: boolean;
    encryptedOverview: string; 
    encryptedDetails: string, 
    tagsIds: string[]; 
    iconId: string;
}
