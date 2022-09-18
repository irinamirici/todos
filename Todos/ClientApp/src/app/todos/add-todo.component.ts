import { Component, Output, EventEmitter } from '@angular/core';
import { ToastrService } from '../services/toastr.service';
import { TodosService } from '../services/todos.service';
import { Todo } from './todo';

@Component({
  selector: 'add-todo',
  templateUrl: './add-todo.component.html'
})
export class AddTodoComponent {
  @Output() cancelAdd = new EventEmitter();
  @Output() todoAdded = new EventEmitter<Todo>();
  loading: boolean = false;
  description: string = '';
  mouseOverSave: boolean = false;

  constructor(private service: TodosService, private toastr: ToastrService) { }

  cancel() {
    this.cancelAdd.emit();
  }

  create(formValues: any) {
    this.loading = true;
    this.service.create(formValues.description)
      .subscribe((result: Todo) => {
        this.loading = false;
        if (result !== undefined) {
          this.toastr.showSuccess("Successfully created");
          this.todoAdded.emit(result);
        }
      });
  }
}