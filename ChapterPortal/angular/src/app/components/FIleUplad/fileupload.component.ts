import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { FolderModelDto, FileModelDto } from '../../shared/model/Announcement.model'

import { AnnouncementService } from '../announcement.service';
@Component({
    selector: 'app-fileupload',
    templateUrl: './fileupload.component.html'
})
export class FileUploadComponent implements OnInit {
    Folders: Array<FolderModelDto>;
    Files: any;
    FolderViewData: Array<FolderModelDto>;
    CurrentSelectedFolder:FolderModelDto;
    ValidationMessage: any = {
        type: 'Success',
        MessageText: ''
    };
    ngOnInit(): void {
        this.GetAllFolders();
    }
    formdata(folder: FolderModelDto) {
        let item: Array<FolderModelDto> = [];
        this.Folders.filter(d => d.parentID == folder.folderID).forEach(ele => {
            ele.childFolders = this.formdata(ele);
            item.push(ele);
        });
        return item;
    }
    myFiles: string[] = [];
    nodes = [
        {
            id: 1,
            name: 'root1',
            children: [
                { id: 2, name: 'child1' },
                { id: 3, name: 'child2' }
            ]
        },
        {
            id: 4,
            name: 'root2',
            children: [
                { id: 5, name: 'child2.1' },
                {
                    id: 6,
                    name: 'child2.2',
                    children: [
                        { id: 7, name: 'subsub' }
                    ]
                }
            ]
        }
    ];
    options = {};
    myForm = new FormGroup({
        name: new FormControl('', [Validators.required, Validators.minLength(3)]),
        file: new FormControl('', [Validators.required])
    });

    constructor(private http: HttpClient, private service: AnnouncementService) { }

    get f() {
        return this.myForm.controls;
    }

    onFileChange(event) {

        for (var i = 0; i < event.target.files.length; i++) {
            this.myFiles.push(event.target.files[i]);
        }
    }

    submit() {
        const formData = new FormData();

        for (var i = 0; i < this.myFiles.length; i++) {
            formData.append("file[]", this.myFiles[i]);
        }

        this.http.post('http://localhost:8001/upload.php', formData)
            .subscribe(res => {
                console.log(res);
                alert('Uploaded Successfully.');
            })
    }

    filesPicked(files, event) {
        for (let i = 0; i < files.length; i++) {
            const file = files[i];
            const path = file.webkitRelativePath.split('/')[0];
            var IsFolderUpload = path != "";
			var FolderId = this.CurrentSelectedFolder == undefined ?0:this.CurrentSelectedFolder.folderID
            this.service.Savefile({ File: file ,FolderId:FolderId,IsFolderUpload:IsFolderUpload }).subscribe((res) => {
            this.GetAllFolders();
            (<HTMLInputElement>document.getElementById("fileUpload")).value = "";
            (<HTMLInputElement>document.getElementById("FolderUpload")).value = "";
            this.ValidationMessage.MessageText ='Files uploaded successfully.';
            this.ValidationMessage.type = 'Success';
            this.ResponseMessage();
             });
        }
    }

    CurrentSelectFolderTree(SelectedFolder:any){
       this.CurrentSelectedFolder = SelectedFolder;
    }

    GetAllFolders(){
        this.service.GetFolders().subscribe((res: Array<FolderModelDto>) => {
            this.Folders = res;
            this.Folders.forEach(ele => {
                ele.childFolders = [];
            })
            this.FolderViewData = [];
            this.Folders.filter(f => f.parentID == 0).forEach(element => {
                element.childFolders = this.formdata(element)
                this.FolderViewData.push(element);
            })
        });
    }
    ResponseMessage() {
        setTimeout(() => {
            this.ValidationMessage.MessageText = '';
        }, 3000);
    }
}