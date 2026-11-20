export interface DekResponse {
    encryptedDek: string, 
    clientSalt: string; 
    cryptoVersion: number;
    login: string;
}