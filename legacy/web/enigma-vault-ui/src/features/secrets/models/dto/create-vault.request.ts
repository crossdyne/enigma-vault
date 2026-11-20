export interface CreateVaultItemRequest {
    vaultType: number;
    iconId: string;
    encryptedOverview: string;
    encryptedDetails: string;
    cryptoVersion: number;
}