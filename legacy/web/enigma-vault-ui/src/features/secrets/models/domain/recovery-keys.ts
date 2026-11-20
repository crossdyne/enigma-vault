export interface RecoveryKeys {
    Application?: string;
    DateCreation?: string;
    DateExpire?: string;
    Keys?: RecoveryKey[];
}

export interface RecoveryKey {
    IsUsed: boolean;
    Key: string;
}
