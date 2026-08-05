import { Component, input, output, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { Task } from '../../../../shared/models/task';

@Component({
  selector: 'app-task-card',
  imports: [DatePipe],
  templateUrl: './task-card.html',
  styleUrl: './task-card.css',
})
export class TaskCard {
  readonly task = input.required<Task>();

  readonly complete = output<Task>();
  readonly edit = output<Task>();
  readonly delete = output<Task>();
  readonly view = output<Task>();

  protected readonly confirmingDelete = signal(false);

  askDelete(): void {
    this.confirmingDelete.set(true);
  }

  cancelDelete(): void {
    this.confirmingDelete.set(false);
  }

  confirmDelete(): void {
    this.delete.emit(this.task());
    this.confirmingDelete.set(false);
  }
}