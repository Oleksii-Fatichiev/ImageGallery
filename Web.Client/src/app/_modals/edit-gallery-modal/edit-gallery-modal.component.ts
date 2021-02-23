import { EventEmitter, Output, TemplateRef } from '@angular/core';
import { ViewChild } from '@angular/core';
import { Component} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-edit-gallery-modal',
  templateUrl: './edit-gallery-modal.component.html'
})

export class ModalEditGalleryComponent{
  editForm: FormGroup;
  closeResult: string;
  redirect = '/login';

  @ViewChild('classic') public templateref: TemplateRef<any>;
  @Output() data: EventEmitter<any> = new EventEmitter();

  get f() { return this.editForm.controls; }

  constructor(private modalService: NgbModal, private formBuilder: FormBuilder) {
    this.editForm = this.formBuilder.group({
      title: ['', [Validators.required, Validators.pattern('^[a-zA-Z0-9 ]*$')]],
      description: ['', [Validators.required, Validators.maxLength(250)]]
    });
  };

  open() {
    this.modalService.open(this.templateref, { centered: true }).result.then((result) => {
      this.closeResult = 'Closed with: $result';
      this.data.emit({title: this.editForm.controls['title'].value, description: this.editForm.controls['description'].value});
    }, (reason) => {
      this.closeResult = 'Dismissed $this.getDismissReason(reason)';
    });
  }
}
