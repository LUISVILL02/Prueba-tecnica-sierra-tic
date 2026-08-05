import { Component, inject, output } from '@angular/core';
import { DatePipe } from '@angular/common';
import { TodoManagement } from '../services/todo-management';

@Component({
  selector: 'app-task-detail-modal',
  imports: [DatePipe],
  templateUrl: './task-detail-modal.html',
  styleUrl: './task-detail-modal.css',
  host: {
    '(document:keydown.escape)': 'closeModal()',
  },
})
export class TaskDetailModal {
  readonly close = output<void>();
  protected readonly service = inject(TodoManagement);

  closeModal(): void {
    this.service.closeDetail();
    this.close.emit();
  }

  onBackdrop(event: MouseEvent): void {
    if (event.target === event.currentTarget) {
      this.closeModal();
    }
  }
}