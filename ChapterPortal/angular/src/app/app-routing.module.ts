import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './helper/auth.guard';
import { AppComponent } from './app.component';

const routes: Routes = [
  { path: '', loadChildren: () => import(`../app/components/announcement.module`).then(m => m.AnnouncementModule) ,canActivate: [AuthGuard]},

  { path: 'chapter', loadChildren: () => import(`../app/components/announcement.module`).then(m => m.AnnouncementModule) ,canActivate: [AuthGuard]},
  
  { path: 'login', component: LoginComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
