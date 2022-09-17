import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public forecasts: Todo[] = [];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Todo[]>(baseUrl + 'api/todos').subscribe(result => {
      this.forecasts = result;
    }, error => console.error(error));
  }
}

interface Todo {
  id: number;
  description: string;
  isDone: boolean;
  createdAt: string;
}
