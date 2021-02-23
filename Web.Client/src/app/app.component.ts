import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/_services/auth.service';
import { GalleryService } from 'src/app/_services/gallery.service';
import { Gallery } from './_models/gallery';
import { User } from './_models/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  currentUser: User;
  
  constructor(
    private router: Router,
    private authService: AuthService,
    private galleryService: GalleryService
  ) {
    this.authService.currentUser.subscribe(x => this.currentUser = x);
  }

  // title = 'imageGaleery-app';
  // galleries: Gallery[];

  ngOnInit() {
    // this.getGalleries();
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}