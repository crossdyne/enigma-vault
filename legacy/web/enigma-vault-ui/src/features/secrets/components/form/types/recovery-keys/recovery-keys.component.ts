import { Component, model } from "@angular/core";
import { RecoveryKeys } from "../../../../models/domain/recovery-keys";

@Component({
    selector: 'recovery-keys-form',
    templateUrl: './recovery-keys.component.html',
    styleUrls: ['./recovery-keys.component.scss'],
    standalone: true,
})
export class RecoveryKeysFormComponent {
    data = model.required<RecoveryKeys>({});
    
    addKey() {
        this.data.update(v => ({
            ...v,
            Keys: [...(v.Keys || []), { Key: '', IsUsed: false }]
        }));
    }

    removeKey(index: number) {
        this.data.update(v => ({...v, Keys: (v.Keys ?? []).filter((_, i) => i !== index) 
        }));
    }

    updateKey(index: number, value: string) {
        this.data.update(v => {
            const newKeys = [...(v.Keys ?? [])]; 
            newKeys[index] = { ...newKeys[index], Key: value };
            return { ...v, Keys: newKeys };
        });
    }
}