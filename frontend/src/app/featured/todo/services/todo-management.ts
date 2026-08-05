import { Service, effect, signal, computed } from '@angular/core';
import { httpResource, HttpErrorResponse } from '@angular/common/http';

import { environment } from '../../../../environments/environment';
import { CreateTaskInput, Task } from '../../../shared/models/task';

export interface UpdateRequest {
  id: number;
  payload: CreateTaskInput;
}

@Service()
export class TodoManagement {
  private readonly base = environment.API_BASE_URL;

  private readonly taskToCreate = signal<CreateTaskInput | undefined>(undefined);
  private readonly taskToUpdate = signal<UpdateRequest | undefined>(undefined);
  private readonly completeTaskId = signal<number | undefined>(undefined);
  private readonly deleteTaskId = signal<number | undefined>(undefined);
  private readonly detailTaskId = signal<number | undefined>(undefined);

  readonly list = httpResource<Task[]>(() => ({ url: this.base, method: 'GET' }));

  readonly detail = httpResource<Task>(() => {
    const id = this.detailTaskId();
    return id != null ? { url: `${this.base}/${id}`, method: 'GET' } : undefined;
  });

  readonly createResource = httpResource<Task>(() => {
    const data = this.taskToCreate();
    return data ? { url: this.base, method: 'POST', body: data } : undefined;
  });

  readonly updateResource = httpResource<Task>(() => {
    const data = this.taskToUpdate();
    return data ? { url: `${this.base}/${data.id}`, method: 'PUT', body: data.payload } : undefined;
  });

  readonly completeResource = httpResource<unknown>(() => {
    const id = this.completeTaskId();
    return id != null ? { url: `${this.base}/${id}/complete`, method: 'PATCH' } : undefined;
  });

  readonly deleteResource = httpResource<unknown>(() => {
    const id = this.deleteTaskId();
    return id != null ? { url: `${this.base}/${id}`, method: 'DELETE' } : undefined;
  });

  readonly mutationResources = [
    this.createResource,
    this.updateResource,
    this.completeResource,
    this.deleteResource,
  ];

  readonly detailOpen = computed(() => this.detailTaskId() !== undefined);

  readonly mutationError = computed(() => {
    for (const resource of this.mutationResources) {
      if (resource.status() === 'error') {
        return this.messageFrom(resource.error());
      }
    }
    return null;
  });

  constructor() {
    effect(() => {
      for (const resource of this.mutationResources) {
        if (resource.status() === 'resolved') {
          this.list.reload();
          this.resetTrigger(resource);
        }
      }
    });
  }

  private resetTrigger(resource: unknown): void {
    if (resource === this.createResource) this.taskToCreate.set(undefined);
    if (resource === this.updateResource) this.taskToUpdate.set(undefined);
    if (resource === this.completeResource) this.completeTaskId.set(undefined);
    if (resource === this.deleteResource) this.deleteTaskId.set(undefined);
  }

  private messageFrom(error: unknown): string {
    if (error instanceof HttpErrorResponse) {
      const body = error.error as { message?: string } | null;
      if (body && typeof body.message === 'string' && body.message.length > 0) {
        return body.message;
      }
    }
    return 'No se pudo completar la operación.';
  }

  create(payload: CreateTaskInput): void {
    this.taskToCreate.set(payload);
  }

  update(id: number, payload: CreateTaskInput): void {
    this.taskToUpdate.set({ id, payload });
  }

  complete(id: number): void {
    this.completeTaskId.set(id);
  }

  delete(id: number): void {
    this.deleteTaskId.set(id);
  }

  openDetail(id: number): void {
    this.detailTaskId.set(id);
  }

  closeDetail(): void {
    this.detailTaskId.set(undefined);
  }

  reloadDetail(): void {
    this.detail.reload();
  }

  reloadList(): void {
    this.list.reload();
  }
}