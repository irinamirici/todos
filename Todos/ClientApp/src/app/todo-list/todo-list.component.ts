import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Todo } from '../todos/todo';

@Component({
  selector: 'app-todo-list',
  templateUrl: './todo-list.component.html',
  styleUrls: ['./todo-list.css']
})
export class TodoListComponent {
  public todos: Todo[] = [];
  public search: string = '';

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Todo[]>(baseUrl + 'api/todos').subscribe(result => {
      this.todos = result;
    }, error => console.error(error));
  }

  doSearch() {
    console.log('search ' + this.search);
  }
}


