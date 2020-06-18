import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import {AnnouncementComponent} from '../components/announcement/announcement.component';
import { EditorModule } from '@tinymce/tinymce-angular';
import { AnnouncementRoutingModule } from './announcement.routing.module';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import {chapterOfficeEditComponent} from '../components/ChapterOffice/chapterOfficeEdit.component'; 
import {AppValidationPupupComponent} from '../shared/components/app-validation-popup.component';
import {ChapterPortalComponent} from '../components/ChapterPortal.component';
import {ResourceComponent} from '../components/resources/resource.component'
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import {FileUploadComponent} from '../components/FIleUplad/fileupload.component';
import { TreeModule } from 'angular-tree-component';
import {TreeviewComponent} from '../components/treeview/treeview.component'
// NOT RECOMMENDED (Angular 9 doesn't support this kind of import)
// import { BsDatepickerModule } from 'ngx-bootstrap';
@NgModule({
    declarations: [
        // , UserAuditLogComponent    We are reffering this component in 'app/core/layout/left-layout.component'
        AnnouncementComponent
        ,chapterOfficeEditComponent
        ,AppValidationPupupComponent
        ,ChapterPortalComponent
        ,ResourceComponent
        ,FileUploadComponent
        ,TreeviewComponent
    ],
    imports: [
        FormsModule
        ,ReactiveFormsModule
        ,HttpClientModule
        , CommonModule
        ,EditorModule
        ,AnnouncementRoutingModule
        ,BsDatepickerModule.forRoot()
        ,NgMultiSelectDropDownModule.forRoot()
        ,TreeModule.forRoot()

    ],

    providers: [
        
    ]
})
export class AnnouncementModule { }