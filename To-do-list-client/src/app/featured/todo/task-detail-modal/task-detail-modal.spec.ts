import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClientTesting, HttpTestingController } from '@angular/common/http/testing';
import { vi } from 'vitest';

import { TaskDetailModal } from './task-detail-modal';
import { TodoManagement } from '../services/todo-management';
import { Task } from '../../../shared/models/task';

const mockTask: Task = {
  id: 7,
  title: 'Revisar PR',
  description: 'Incluir observaciones del reviewer',
  isCompleted: false,
  createdAt: '2026-08-05T10:00:00Z',
};

describe('TaskDetailModal', () => {
  let fixture: ComponentFixture<TaskDetailModal>;
  let component: TaskDetailModal;
  let service: TodoManagement;
  let httpTesting: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TaskDetailModal],
      providers: [provideHttpClientTesting()],
    }).compileComponents();

    fixture = TestBed.createComponent(TaskDetailModal);
    component = fixture.componentInstance;
    service = TestBed.inject(TodoManagement);
    httpTesting = TestBed.inject(HttpTestingController);
  });

  it('is empty when detail is closed', async () => {
    fixture.detectChanges();
    httpTesting.expectOne((req) => req.method === 'GET' && req.url.endsWith('/task')).flush([]);
    await fixture.whenStable();
    fixture.detectChanges();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('.modal-backdrop')).toBeNull();
  });

  it('renders the task detail once opened and loaded', async () => {
    service.openDetail(mockTask.id);
    fixture.detectChanges();
    httpTesting.expectOne((req) => req.method === 'GET' && req.url.endsWith('/task')).flush([]);
    httpTesting
      .expectOne((req) => req.method === 'GET' && req.url.endsWith(`/task/${mockTask.id}`))
      .flush(mockTask);
    await fixture.whenStable();
    fixture.detectChanges();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('.modal__title')?.textContent).toContain('Detalle de la tarea');
    expect(compiled.textContent).toContain('Revisar PR');
    expect(compiled.querySelector('.modal__close')).toBeTruthy();
  });

  it('closes and emits close when clicking the close button', async () => {
    service.openDetail(mockTask.id);
    fixture.detectChanges();
    httpTesting.expectOne((req) => req.method === 'GET' && req.url.endsWith('/task')).flush([]);
    httpTesting
      .expectOne((req) => req.method === 'GET' && req.url.endsWith(`/task/${mockTask.id}`))
      .flush(mockTask);
    await fixture.whenStable();
    fixture.detectChanges();

    vi.spyOn(component.close, 'emit');
    const closeBtn = (fixture.nativeElement as HTMLElement).querySelector(
      '.modal__close',
    ) as HTMLButtonElement;
    closeBtn.click();
    fixture.detectChanges();

    expect(service.detailOpen()).toBe(false);
    expect((fixture.nativeElement as HTMLElement).querySelector('.modal-backdrop')).toBeNull();
    expect(component.close.emit).toHaveBeenCalled();
  });

  it('shows an error state and retry button on failure', async () => {
    service.openDetail(mockTask.id);
    fixture.detectChanges();
    httpTesting.expectOne((req) => req.method === 'GET' && req.url.endsWith('/task')).flush([]);
    httpTesting
      .expectOne((req) => req.method === 'GET' && req.url.endsWith('/task/7'))
      .flush('Task not found', { status: 404, statusText: 'Not Found' });
    await fixture.whenStable();
    fixture.detectChanges();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('No se pudo cargar la tarea.');
    expect(compiled.querySelector('.modal__retry')).toBeTruthy();
  });
});