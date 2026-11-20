export interface UpdateVaultItemRequest {
    vaultItemId: string;
    iconId: string; 
    encryptedOverview: string;
    encryptedDetails: string;
    cryptoVersion: number;
}