/// <reference lib="webworker" />

import { CryptoService, CryptoVersion, KeyDerivationService, SecurityUtils } from "@crossdyne/security";

const ctx = self as DedicatedWorkerGlobalScope;

let dek: CryptoKey | null = null;
let cryptoVersion: CryptoVersion | null = null;

const keyDerivation = new KeyDerivationService();
const cryptoService = new CryptoService();

function secureWipe(buffer: Uint8Array | null): void {
  if (buffer) 
    buffer.fill(0);
}

ctx.addEventListener('message', async (event) => {
  const { action, payload, id } = event.data;

  try {
    switch (action) {
        case 'init': {
            if (dek) {
                throw new Error('Worker уже инициализирован. Сначала вызовите clear().');
            }

            const { login, password, clientSalt, encryptedDek, version } = payload;
            const salt = SecurityUtils.fromBase64(clientSalt);
            
            const { kek } = await keyDerivation.deriveKeysFromPassword(
                login.toLowerCase(),
                password,
                salt,
                version as CryptoVersion
            );

            const tempKekCryptoKey = await crypto.subtle.importKey(
                    'raw',
                    kek as BufferSource,
                    'AES-GCM',
                    false,
                    ['decrypt']
                );

            const rawDek = await cryptoService.decryptData<Uint8Array>(encryptedDek, tempKekCryptoKey, true);
            
            if (!rawDek) 
                throw new Error('При дешифрование DEK произошла ошибка. Вернулся null');

            dek = await cryptoService.importKey(rawDek, version as CryptoVersion, ['encrypt', 'decrypt']);
            cryptoVersion = version as CryptoVersion;

            secureWipe(kek);
            secureWipe(rawDek);

            ctx.postMessage({ id, action: 'init', success: true });
            break;
        }

        case 'decrypt': {
            if (!dek) 
                throw new Error('Worker не был инициализирован');

            const { encryptedData, isBytes } = payload;
            
            const result = await cryptoService.decryptData(encryptedData, dek, isBytes ?? false);

            ctx.postMessage({ 
                id, 
                action: 'decrypt', 
                success: true, 
                data: result 
            });
            break;
        }
        
        case 'encrypt': {
            if (!dek) 
                throw new Error('Worker не был инициализирован');
            
            const { data, version } = payload;
            const encrypted = await cryptoService.encryptData(data, dek, version as CryptoVersion);

            ctx.postMessage({ 
                id, 
                action: 'encrypt', 
                success: true, 
                encryptedData: encrypted,
                cryptoVersion: version
            });
            break;
        }
        
        case 'clear': {
            dek = null;
            cryptoVersion = null;

            ctx.postMessage({ id, action: 'clear', success: true });
            break;
        }
        
        default:
            throw new Error(`Неизвестное действие: ${action}`);
        }
    } catch (error) {
        const msg = error instanceof Error ? error.message : 'Ошибка CryptoWorker';
        console.error('[CryptoWorker]', action, msg);
        ctx.postMessage({ id, action, success: false, error: msg });
    }
});