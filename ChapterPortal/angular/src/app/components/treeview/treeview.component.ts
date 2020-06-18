import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators} from '@angular/forms';

import { AnnouncementService } from '../announcement.service'
@Component({
    selector: 'app-treeview',
    templateUrl: './treeview.component.html'
})
export class TreeviewComponent{
@Input() FolderViewData:any;
@Output() CurrentSelectFolder = new EventEmitter();
constructor(){

}
SelectFolder(folderdata:any){
    this.CurrentSelectFolder.emit(folderdata);
}

}