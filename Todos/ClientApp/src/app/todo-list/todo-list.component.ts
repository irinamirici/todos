import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Todo } from '../todos/todo';

@Component({
  selector: 'app-todo-list',
  templateUrl: './todo-list.component.html',
  styleUrls: ['./todo-list.css']
})
export class TodoListComponent implements OnInit {
  public todos: Todo[] = [];
  public search: string = '';
  baseUrl: string;
  http: HttpClient;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
    this.http = http;
  }

  ngOnInit(): void {
    this.doSearch();
  }

  doSearch() {
    const headerDict = {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
    }
    const requestOptions = {
      headers: new HttpHeaders(headerDict),
    };
    let body = {
      searchTerm: this.search
    }
    console.log('search ' + this.search);
    this.http.post<Todo[]>(this.baseUrl + 'api/todos', body, requestOptions).subscribe(result => {
      this.todos = result;
    }, error => console.error(error));

    this.search = "done";
  }
}


