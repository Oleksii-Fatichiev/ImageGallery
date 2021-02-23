import { ImagePipe } from './image.pipe';

import { SafePipe } from './safe.pipe';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';



@NgModule({
  declarations: [SafePipe, ImagePipe],
  exports: [SafePipe, ImagePipe],
  imports: [
    CommonModule
  ]
})
export class PipesModule { }
