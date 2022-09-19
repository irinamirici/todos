import { Injectable } from "@angular/core";
import { catchError, Observable, of } from "rxjs";
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Todo } from "../todos/todo";
import { ToastrService } from "./toastr.service";
import { Paged } from "../shared/paged";

const requestOptions = {
    headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json'
    }),
};

@Injectable({
    providedIn: 'root'
})
export class TodosService {
    constructor(private http: HttpClient, private toastr: ToastrService) { }

    getTodos(searchTerm: string, page: number, itemsPerPage: number): Observable<Paged<Todo>> {
        let body = {
            searchTerm: searchTerm,
            page: page,
            itemsPerPage: itemsPerPage
        }

        return this.http.post<Paged<Todo>>(
            `api/todos/search`,
            body,
            requestOptions)
            .pipe(catchError(this.handleError<Paged<Todo>>('getTodos')));
    }

    create(description: string): Observable<Todo> {
        return this.http.post<Todo>(
            `api/todos`,
            {
                description: description
            },
            requestOptions)
            .pipe(catchError(this.handleError<Todo>('create')));
    }

    markAsDone(id: string): Observable<Todo> {
        return this.http.put<Todo>(
            `api/todos/${id}/status`,
            { isDone: true },
            requestOptions)
            .pipe(catchError(this.handleError<Todo>('markAsDone')));
    }

    delete(id: string): Observable<any> {
        return this.http.delete(
            `/api/todos/${id}`,
            requestOptions)
            .pipe(catchError(this.handleError<any>('delete')));
    }

    private handleError<T>(operation = 'operation', result?: T) {
        return (error: any): Observable<T> => {
            let errorMessage = "An error has occured: ";
            if (error.error instanceof Error) {
                // A client-side or network error occurred. Handle it accordingly.
                errorMessage += error.error.message;
            } else {
                // The backend returned an unsuccessful response code.
                // The response body may contain clues as to what went wrong,
                errorMessage = `${error.error} (${error.status})`;
            }
            this.toastr.showError(errorMessage, operation);
            return of(result as T);
        }
    }
}
