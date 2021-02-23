import { Routes, RouterModule } from '@angular/router';

import { AuthGuard } from './app/_helpers/auth.guard';

import { LoginComponent } from './app/components/login';
import { GalleryComponent } from './app/components/gallery';
import { PictureComponent } from './app/components/picture';
import { RegisterComponent } from './app/components/register';

const routes: Routes = [
    { path: '', component: GalleryComponent, canActivate: [AuthGuard] },
    { path: 'picture', component: PictureComponent, canActivate: [AuthGuard] },
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },

    // otherwise redirect to home
    { path: '**', redirectTo: '' }
];

export const appRoutingModule = RouterModule.forRoot(routes);
