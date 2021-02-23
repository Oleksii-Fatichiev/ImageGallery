import { TemplateRef } from '@angular/core';
import { ViewChild } from '@angular/core';
import { Component} from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal} from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'app-modal',
    templateUrl: './register-modal.component.html',
    styleUrls: ['./register-modal.component.scss']
})
export class ModalComponent{
    closeResult: string;
    redirect = '/login';

    @ViewChild('classic') public templateref: TemplateRef<any>;

    constructor(private modalService: NgbModal, private router: Router) { }

    open() {
        this.modalService.open(this.templateref, { centered: true }).result.then((result) => {
            this.closeResult = 'Closed with: $result';
            this.router.navigate([this.redirect]);
        }, (reason) => {
            this.closeResult = 'Dismissed $this.getDismissReason(reason)';
        });
    }
}
