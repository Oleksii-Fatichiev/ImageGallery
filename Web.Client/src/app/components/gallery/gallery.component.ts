import { Component, ViewChild } from '@angular/core';
import { Router } from '@angular/router';

import { environment } from 'src/environments/environment';

import { Gallery } from 'src/app/_models/gallery';
import { GalleryService } from 'src/app/_services/gallery.service';
import { ModalEditGalleryComponent } from 'src/app/_modals/edit-gallery-modal';


@Component({
  selector: 'app-gallery',
  templateUrl: './gallery.component.html',
  styleUrls: ['./gallery.component.scss']
})
export class GalleryComponent {
  loading: boolean = false;
  galleries: Gallery[];
  editId: number;
  picPath: string;

  @ViewChild(ModalEditGalleryComponent) private modal: ModalEditGalleryComponent;

  constructor(private galleryService: GalleryService,
    private route: Router) {
  }

  ngOnInit() {
    this.loading = true;
    this.picPath = `${environment.apiUrl}`;/// todo
    this.getGallery();
  }

  getGallery() {
    this.galleryService.getGalleries().subscribe((resp) => {
      this.loading = false;
      this.galleries = resp;
    });
  }

  galleryEdit(id: number) {
    this.editId = id;
    this.modal.open();
  }

  galleryDelete(id: number) {
    this.galleryService.deleteGalleries([id]).subscribe((resp) => {
      if (resp) {
        const index = this.galleries.findIndex(el => el.id === id);
        this.galleries.splice(index, 1);
      }
    }, (err) => { });
  }

  addNewGallery() {
    this.editId = undefined;
    this.modal.open();
  }

  updateOrCreateGallery(event: any) {
    if(this.editId){
      this.galleryService.updateGallery(this.editId, event.title, event.description).subscribe(() => {
        this.getGallery();
      },(err)=>{
        
      })
    } else {
      this.galleryService.createGallery(event.title, event.description).subscribe(() => {
        this.getGallery();
      }, (err)=>{

      })
    }
  }

  goToPictures(id: number){
    this.route.navigate(['/picture'], { queryParams: { 'id' : id }});
  }
}
