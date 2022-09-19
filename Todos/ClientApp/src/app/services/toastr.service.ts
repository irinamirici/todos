import { Injectable } from "@angular/core";

declare let toastr: any;

@Injectable({
    providedIn: 'root'
})
export class ToastrService {
    constructor() {
        toastr.options.positionClass = 'toast-top-center';
    }

    showSuccess(message: string, title?: string) {
        toastr.success(message, title);
    }

    showError(message: string, title?: string) {
        toastr.error(message, title);
    }
}