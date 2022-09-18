import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Todo } from '../todos/todo';
import { TodosService } from '../services/todos.service';

@Component({
  selector: 'app-todo-list',
  templateUrl: './todo-list.component.html',
  styleUrls: ['./todo-list.css']
})
export class TodoListComponent implements OnInit {
  public todos: Todo[] = [];
  public search: string = '';
  addTodo: boolean = false;

  constructor(private service: TodosService) { }

  ngOnInit(): void {
    this.doSearch();
  }

  doSearch() {
    console.log("do search")
    this.service.getTodos(this.search)
      .subscribe((result) => this.todos = result);
  }

  todoAdded(todo: Todo) {
    this.addTodo = false;
    this.todos.splice(0, 0, todo);
  }

  todoDeleted(id: string) {
    this.removeTodoFromList(id)
  }

  todoDone(id: string) {
    this.removeTodoFromList(id)
  }

  private removeTodoFromList(id: string) {
    let index = this.todos.findIndex(x => x.id === id);
    this.todos.splice(index, 1);//remove element from array
  }
}


