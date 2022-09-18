import { Component, Input, EventEmitter, Output } from '@angular/core';
import { ToastrService } from '../services/toastr.service';
import { TodosService } from '../services/todos.service';
import { Todo } from './todo';

@Component({
  selector: 'todo-card',
  templateUrl: './todo-card.component.html',
  styleUrls: ['./todo-card.css']
})
export class TodoCardComponent {
  @Input() todo!: Todo;
  @Output() todoDeleted = new EventEmitter<string>();
  @Output() todoDone = new EventEmitter<string>();
  loading: boolean = false;

  constructor(private service: TodosService, private toastr: ToastrService) { }

  markAsDone() {
    this.loading = true;
    this.service.markAsDone(this.todo.id)
      .subscribe((result) => {
        this.loading = false;
        if (result !== undefined) {
          this.toastr.showSuccess("Marked as done");
          this.todoDone.emit(this.todo.id);
        }
      });
  }

  delete() {
    this.loading = true;
    this.service.delete(this.todo.id)
      .subscribe((result) => {
        this.loading = false;
        if (result !== undefined) {
          this.toastr.showSuccess("Successfully deleted");
          this.todoDeleted.emit(this.todo.id);
        }
      });
  }
}