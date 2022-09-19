import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Todo } from '../todos/todo';
import { TodosService } from '../services/todos.service';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-todo-list',
  templateUrl: './todo-list.component.html',
  styleUrls: ['./todo-list.css']
})
export class TodoListComponent implements OnInit {
  dataSource: MatTableDataSource<Todo> = new MatTableDataSource();
  public search: string = '';
  addTodo: boolean = false;

  pageSizeOptions: number[] = [5, 10, 15];
  currentPage: number = 0;
  itemsPerPage: number = 10;
  totalItems!: number;
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(private service: TodosService) { }

  ngOnInit(): void {
    // load initial data, page 0, no search term provided
    this.doSearch();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  doSearch() {
    console.log(`do search. Page ${this.currentPage}, items per page ${this.itemsPerPage}`)
    this.service.getTodos(this.search, this.currentPage, this.itemsPerPage)
      .subscribe((result) => {
        this.dataSource.data = result.data;
        this.totalItems = result.totalCount;
        setTimeout(() => {
          this.paginator.pageIndex = this.currentPage;
          this.paginator.length = this.totalItems;
        });
      });
  }

  pageChanged(event: PageEvent) {
    console.log({ event });
    this.itemsPerPage = event.pageSize;
    this.currentPage = event.pageIndex;
    this.doSearch();
  }

  todoAdded(todo: Todo) {
    this.addTodo = false;
    this.dataSource.data.splice(0, 0, todo);
  }

  todoDeleted(id: string) {
    this.removeTodoFromList(id)
  }

  todoDone(id: string) {
    let index = this.dataSource.data.findIndex(x => x.id === id);
    this.dataSource.data[index].isDone = true;
  }

  private removeTodoFromList(id: string) {
    let index = this.dataSource.data.findIndex(x => x.id === id);
    this.dataSource.data.splice(index, 1);//remove element from array
  }
}


