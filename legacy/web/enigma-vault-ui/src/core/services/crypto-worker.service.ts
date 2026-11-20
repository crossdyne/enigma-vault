import { Injectable, OnDestroy, NgZone, inject } from '@angular/core';
import { CryptoVersion } from '@crossdyne/security';

interface WorkerResponse {
  id: number;
  action: string;
  success: boolean;
  data?: any;
  encryptedData?: string;
  cryptoVersion?: number;
  error?: string;
}

@Injectable({ providedIn: 'root' })
export class CryptoWorkerService implements OnDestroy {
    private zone = inject(NgZone);

    private worker: Worker;
    private id = 0;
    private pending = new Map<number, { resolve: (v: WorkerResponse) => void; reject: (e: Error) => void }>();
    private _initialized = false;
    
    constructor() {
        this.worker = new Worker(new URL('../workers/crypto.worker', import.meta.url), { type: 'module' });

        this.worker.onmessage = ({ data }: MessageEvent<WorkerResponse>) => {
            this.zone.run(() => {
                const resolver = this.pending.get(data.id);

                if (!resolver) 
                    return;

                this.pending.delete(data.id);
                
                if (data.success) {
                    resolver.resolve(data);
                } else {
                    resolver.reject(new Error(data.error || 'Ошибка CryptoWorker'));
                }
            });
        };
        
        this.worker.onmessageerror = (err) => {
            console.error('Worker message error:', err);
            this.zone.run(() => this.rejectAllPending(new Error('Worker вернул сообщение об ошибке')));
        };

        this.worker.onerror = (err) => {
            console.error('Worker runtime error:', err.message);
            this.zone.run(() => this.rejectAllPending(new Error(`Ошибка Worker во время выполнения: ${err.message}`)));
        };
    }

    private rejectAllPending(error: Error): void {
        for (const [, resolver] of this.pending) { 
            resolver.reject(error); 
        }
        this.pending.clear();
    }

    get initialized(): boolean {
        return this._initialized;
    }

    private post(action: string, payload?: any, timeoutMs = 30000): Promise<WorkerResponse> {
        return new Promise<WorkerResponse>((resolve, reject) => {
            const reqId = ++this.id;
            this.pending.set(reqId, { resolve, reject });

            const timer = setTimeout(() => {
                this.pending.delete(reqId);
                reject(new Error(`Ошибка Worker во время ожидание действия: ${action}`));
            }, timeoutMs);

            const wrappedResolve = (v: WorkerResponse) => {
                clearTimeout(timer);
                resolve(v);
            };

            const wrappedReject = (e: Error) => {
                clearTimeout(timer);
                reject(e);
            };

            this.pending.set(reqId, { resolve: wrappedResolve, reject: wrappedReject });
            this.worker.postMessage({ id: reqId, action, payload });
        });
    }

    async init(login: string, password: string, clientSalt: string, encryptedDek: string, version: CryptoVersion): Promise<void> {
        await this.post('init', { login, password, clientSalt, encryptedDek, version }, 60000);
        this._initialized = true;
    }

    decrypt<T>(encryptedData: string): Promise<T> {
        return this.post('decrypt', { encryptedData, isBytes: false }).then(r => r.data as T);
    }

    encrypt(data: any, version: number): Promise<{ encryptedData: string; cryptoVersion: number }> {
        return this.post('encrypt', { data, version }).then(r => ({
            encryptedData: r.encryptedData!,
            cryptoVersion: r.cryptoVersion!
        }));
    }

    async clear(): Promise<void> {
        await this.post('clear');
        this._initialized = false;
    }

    ngOnDestroy(): void {
        this.clear().finally(() => this.worker.terminate());
    }
}