import { DIALOG_DATA, DialogRef } from "@angular/cdk/dialog";
import { Component, inject, signal } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { TagAttachmentResult } from "../../models/modal/tag-attachment.result";
import { TagAttachmentData } from "../../models/modal/tag-attachment.data";
import { TagResponse } from "../../models/dto/tag.response";

@Component({
    selector: 'attach-tag',
    templateUrl: './attach-tag.component.html',
    styleUrls: ['./attach-tag.component.scss'],
    standalone: true,
    imports: [
      FormsModule
    ]
})
export class AttachTagComponent {
  private dialogRef = inject(DialogRef<TagAttachmentResult>);
  private data = inject(DIALOG_DATA) as TagAttachmentData;

  vault = this.data.vault;
  availableTags = this.data.availableTags; 
  selectedTagIds = signal<Set<string>>(new Set(this.data.initialSelectedTagIds));
  
  tagsModified = signal(false);

  isCreating = signal(false);
  newTagName = signal('');
  newTagColor = signal('#F0F0F0');

  editingTagId = signal<string | null>(null);
  editName = signal('');
  editColor = signal('');

  toggleTag(tagId: string) {
    this.selectedTagIds.update(set => {
      const newSet = new Set(set);
      if (newSet.has(tagId)) {
        newSet.delete(tagId);
      } else {
        newSet.add(tagId);
      }

      this.tagsModified.set(true);
      
      return newSet;
    });
  }

  isSelected(tagId: string): boolean {
    return this.selectedTagIds().has(tagId);
  }

  startCreate() {
    this.isCreating.set(true);
    this.newTagName.set('');
    this.newTagColor.set('#F0F0F0');
    this.editingTagId.set(null);
  }

  cancelCreate() {
    this.isCreating.set(false);
  }

  async onCreateTag() {
    const name = this.newTagName().trim();
    const color = this.newTagColor();
    if (!name) 
        return;

    const tagId = await this.data.actions.createTag(name, color);

    if (tagId) {
        this.tagsModified.set(true);
        this.isCreating.set(false);
        
        this.selectedTagIds.update(set => {
            const newSet = new Set(set);
            newSet.add(tagId);
            return newSet;
        });
    }
  }

  startEdit(tag: TagResponse) {
    this.editingTagId.set(tag.id);
    this.editName.set(tag.name);
    this.editColor.set(tag.color);
    this.isCreating.set(false);
  }

  cancelEdit() {
    this.editingTagId.set(null);
  }

  async onUpdateTag(id: string) {
    const name = this.editName().trim();
    if (!name) return;

    const success = await this.data.actions.updateTag(id, name, this.editColor());
    if (success) {
      this.tagsModified.set(true);
      this.editingTagId.set(null);
    }
  }

  async onDeleteTag(id: string) {
    const tag = this.availableTags().find(t => t.id === id);
    if (!tag) return;
    
    if (!confirm(`Удалить тег «${tag.name}»? Это действие открепит тег от всех записей.`)) return;

    const success = await this.data.actions.deleteTag(id);
    if (success) {
      this.selectedTagIds.update(set => {
        const newSet = new Set(set);
        newSet.delete(id);
        return newSet;
      });
      this.tagsModified.set(true);
    }
  }

  onCancel() {
    this.dialogRef.close();
  }

  onSave() {
    this.dialogRef.close({
      selectedTagIds: Array.from(this.selectedTagIds()),
      tagsModified: this.tagsModified()
    });
  }
}