import { Component } from '@angular/core';
import { Todo } from './featured/todo/todo';
import { Loader } from './shared/loading/loader';

@Component({
  selector: 'app-root',
  imports: [Todo, Loader],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
}
