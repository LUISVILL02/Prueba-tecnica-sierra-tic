import { Component, inject } from '@angular/core';
import { Loading } from '../services/loading';

@Component({
  selector: 'app-loader',
  templateUrl: './loader.html',
  styleUrl: './loader.css',
})
export class Loader {
  readonly loading = inject(Loading);
}