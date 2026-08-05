import { ComponentFixture, TestBed } from '@angular/core/testing';
import { vi } from 'vitest';

import { TaskCard } from './task-card';
import { Task } from '../../../../shared/models/task';

const mockTask: Task = {
  id: 1,
  title: 'Comprar leche',
  description: 'Ir al supermercado',
  isCompleted: false,
  createdAt: '2026-08-05T10:00:00Z',
};

describe('TaskCard', () => {
  let fixture: ComponentFixture<TaskCard>;
  let component: TaskCard;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TaskCard],
    }).compileComponents();

    fixture = TestBed.createComponent(TaskCard);
    component = fixture.componentInstance;
    fixture.componentRef.setInput('task', mockTask);
    fixture.detectChanges();
  });

  it('should render the task title and description', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('.card__title')?.textContent).toContain('Comprar leche');
    expect(compiled.querySelector('.card__desc')?.textContent).toContain('Ir al supermercado');
  });

  it('shows "Marcar completada" when pending and emits complete on click', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    const button = compiled.querySelector<HTMLButtonElement>('.card__check')!;
    expect(button.textContent).toContain('Marcar completada');

    vi.spyOn(component.complete, 'emit');
    button.click();
    expect(component.complete.emit).toHaveBeenCalledWith(mockTask);
  });

  it('emits "view" on Ver click', () => {
    const button = (fixture.nativeElement as HTMLElement).querySelector<HTMLButtonElement>(
      '.card__action',
    )!;
    vi.spyOn(component.view, 'emit');
    button.click();
    expect(component.view.emit).toHaveBeenCalledWith(mockTask);
  });

  it('asks for confirmation before deleting and emits "delete" on confirm', () => {
    const buttons = (fixture.nativeElement as HTMLElement).querySelectorAll<HTMLButtonElement>(
      '.card__action',
    );
    vi.spyOn(component.delete, 'emit');

    buttons[2].click();
    fixture.detectChanges();

    const dialog = (fixture.nativeElement as HTMLElement).querySelector(
      '.card__confirm-text',
    );
    expect(dialog?.textContent).toContain('¿Eliminar esta tarea?');

    const confirm = (fixture.nativeElement as HTMLElement).querySelector<HTMLButtonElement>(
      '.card__action--danger',
    )!;
    confirm.click();
    expect(component.delete.emit).toHaveBeenCalledWith(mockTask);
  });

  it('shows "Completada" label instead of button when done', () => {
    fixture.componentRef.setInput('task', { ...mockTask, isCompleted: true });
    fixture.detectChanges();

    const label = (fixture.nativeElement as HTMLElement).querySelector('.card__check--done');
    expect(label?.textContent).toContain('Completada');
  });
});