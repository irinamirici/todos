import { Component, Inject, Input } from '@angular/core';
import { Todo } from './todo';

@Component({
  selector: 'todo-card',
  templateUrl: './todo-card.component.html',
  styleUrls: ['./todo-card.css']
})
export class TodoCardComponent {
  @Input() todo!: Todo;
  loading: boolean = false;

  constructor(@Inject('BASE_URL') baseUrl: string) {

  }


}