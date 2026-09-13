import { Signal } from "@angular/core";
import { VaultItemDisplay } from "../domain/vault-item-display";
import { TagResponse } from "../dto/tag.response";

export interface TagAttachmentData {
  vault: VaultItemDisplay;
  availableTags: Signal<TagResponse[]>;
  initialSelectedTagIds: Set<string>;
  actions: {
      createTag: (name: string, color: string) => Promise<string | null>;
      updateTag: (id: string, name: string, color: string) => Promise<boolean>;
      deleteTag: (id: string) => Promise<boolean>;
  };
}