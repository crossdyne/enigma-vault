import { VaultItemDisplay } from "../domain/vault-item-display";

export interface VaultItemFormData {
    mode: 'create' | 'edit';
    item?: VaultItemDisplay; 
    decryptedDetails?: Record<string, any> | null;
}