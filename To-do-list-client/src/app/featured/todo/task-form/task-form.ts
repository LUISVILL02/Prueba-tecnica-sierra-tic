import { Component, computed, input, output, signal } from '@angular/core';
import { form, FormField, required, maxLength, submit } from '@angular/forms/signals';
import { CreateTaskInput, UpdateTaskInput } from '../../../shared/models/task';

@Component({
  selector: 'app-task-form',
  imports: [FormField],
  templateUrl: './task-form.html',
  styleUrl: './task-form.css',
})
export class TaskForm {
  readonly taskId = input<number | null>(null);
  readonly initial = input<CreateTaskInput>({ title: '', description: '' });

  protected readonly create = output<CreateTaskInput>();
  protected readonly update = output<UpdateTaskInput>();
  protected readonly cancel = output<void>();

  protected readonly editing = computed(() => this.taskId() !== null);

  protected readonly model = signal<CreateTaskInput>({
    title: '',
    description: '',
  });

  protected readonly taskForm = form(this.model, (s) => {
    required(s.title, { message: 'El título es obligatorio.' });
    maxLength(s.title, 50, { message: 'Máximo 50 caracteres.' });
    required(s.description, { message: 'La descripción es obligatoria.' });
    maxLength(s.description, 2000, { message: 'Máximo 2000 caracteres.' });
  });

  ngOnInit() {
    const init = this.initial();
    this.model.set({ title: init.title, description: init.description });
  }

  onSubmit() {
    submit(this.taskForm, async () => {
      const payload: CreateTaskInput = {
        title: this.model().title.trim(),
        description: this.model().description.trim(),
      };

      const id = this.taskId();
      if (id === null) {
        this.create.emit(payload);
        this.model.set({ title: '', description: '' });
      } else {
        this.update.emit({ id, payload });
      }
    });
  }
}