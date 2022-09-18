import { Injectable } from "@angular/core";

declare let toastr: any;

@Injectable({
    providedIn: 'root'
})
export class ToastrService {
    showSuccess(message: string, title?: string) {
        toastr.success(message, title);
    }

    showError(message: string, title?: string) {
        toastr.error(message, title);
    }
}