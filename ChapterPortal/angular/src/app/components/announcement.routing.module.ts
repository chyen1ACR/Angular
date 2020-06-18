import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {AnnouncementComponent} from '../components/announcement/announcement.component';
import { chapterOfficeEditComponent} from '../components/ChapterOffice/chapterOfficeEdit.component';
import {ChapterPortalComponent} from '../components/ChapterPortal.component';
import { FileUploadComponent } from './FIleUplad/fileupload.component';

const Announcement_ROUTES: Routes = [
    {
        path: '', component: ChapterPortalComponent
    },
    {
        path: 'myportal', component: FileUploadComponent
    },
    {
        path: 'announcement', component: AnnouncementComponent
    },
    {
        path: 'ChapterOffierEdit', component: chapterOfficeEditComponent
    },
    {
        path:'FileUpload', component:FileUploadComponent
    }
    
];

@NgModule({
    imports: [RouterModule.forChild(Announcement_ROUTES)],
    exports: [RouterModule]
})
export class AnnouncementRoutingModule { }