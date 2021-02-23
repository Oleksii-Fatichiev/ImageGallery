import { ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';

import { PictureService } from 'src/app/_services/picture.service';

@Component({
  selector: 'app-picture',
  templateUrl: './picture.component.html',
  styleUrls: ['./picture.component.scss']
})
export class PictureComponent implements OnInit {
  id: string;
  constructor(private routerActive: ActivatedRoute,
    private pictureService: PictureService) { }

  ngOnInit(): void {
    this.routerActive.params.subscribe((queryParam: any) => {
      this.id = queryParam['id'];
    });
  }

  getPictures() {

  }

  fileChangeEvent(event: any): void {
    console.log(event.target.files[0]as File);
    this.pictureService.createPicture(event.target.files[0] as File).subscribe();
  }
}
