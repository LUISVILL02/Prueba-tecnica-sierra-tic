import { Component, computed, inject, signal } from '@angular/core';
import { CreateTaskInput, Task } from '../../shared/models/task';
import { TodoManagement } from './services/todo-management';
import { TaskCardList } from './task-card-list/task-card-list';
import { TaskForm } from './task-form/task-form';
import { TaskDetailModal } from './task-detail-modal/task-detail-modal';

@Component({
  selector: 'app-todo',
  imports: [TaskForm, TaskCardList, TaskDetailModal],
  templateUrl: './todo.html',
  styleUrl: './todo.css',
})
export class Todo {
  protected readonly service = inject(TodoManagement);

  protected readonly tasks = computed(() =>
    this.service.list.hasValue() ? (this.service.list.value() ?? []) : [],
  );

  protected readonly isListLoading = this.service.list.isLoading;

  protected readonly editingId = signal<number | null>(null);

  protected readonly editingTask = computed(() => {
    const id = this.editingId();
    return id === null ? null : (this.tasks().find((t) => t.id === id) ?? null);
  });

  protected readonly editingInitial = computed<CreateTaskInput>(() => {
    const task = this.editingTask();
    return task ? { title: task.title, description: task.description } : { title: '', description: '' };
  });

  protected readonly doneCount = computed(() => this.tasks().filter((t) => t.isCompleted).length);
  protected readonly openCount = computed(() => this.tasks().length - this.doneCount());

  onCreateTask(payload: CreateTaskInput) {
    this.service.create(payload);
  }

  onComplete(task: Task) {
    this.service.complete(task.id);
  }

  onEdit(task: Task) {
    this.editingId.set(task.id);
  }

  onCancelEdit() {
    this.editingId.set(null);
  }

  onUpdateTask(payload: CreateTaskInput) {
    const id = this.editingId();
    if (id !== null) {
      this.service.update(id, payload);
      this.editingId.set(null);
    }
  }

  onDelete(task: Task) {
    this.service.delete(task.id);
  }

  onView(task: Task) {
    this.service.openDetail(task.id);
  }

  reload() {
    this.service.reloadList();
  }
}