import { input, output } from '@angular/core';
import { Component } from '@angular/core';
import { Task } from '../../../shared/models/task';
import { TaskCard } from './task-card/task-card';

@Component({
  selector: 'app-task-card-list',
  imports: [TaskCard],
  templateUrl: './task-card-list.html',
  styleUrl: './task-card-list.css',
})
export class TaskCardList {
  readonly tasks = input.required<Task[]>();

  readonly complete = output<Task>();
  readonly edit = output<Task>();
  readonly delete = output<Task>();
  readonly view = output<Task>();
}