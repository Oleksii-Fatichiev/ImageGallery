import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { appRoutingModule } from 'src/app.routing';
import { JwtInterceptor } from './_helpers/jwt.interceptor';
import { ErrorInterceptor } from './_helpers/error.interceptor';

import { PipesModule } from './_pipes/pipes.module';

import { AppComponent } from './app.component';
import { ModalComponent } from './_modals/register-modal/register-modal.component';
import { ModalEditGalleryComponent } from './_modals/edit-gallery-modal/edit-gallery-modal.component';
import { LoginComponent } from './components/login';
import { GalleryComponent } from './components/gallery';
import { RegisterComponent } from './components/register';
import { PictureComponent } from './components/picture';


@NgModule({
  declarations: [	
    AppComponent,
    GalleryComponent,
    LoginComponent,
    RegisterComponent,
    ModalComponent,
    ModalEditGalleryComponent,
    PictureComponent
   ],
  imports: [
    BrowserModule,
    HttpClientModule,
    ReactiveFormsModule,
    appRoutingModule,
    NgbModule,
    PipesModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
