import { Component, OnInit } from '@angular/core';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker/public_api';
import { CookieService } from 'ngx-cookie-service';
import { AnnouncementService } from '../announcement.service';
import { Router, ActivatedRoute } from '@angular/router';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
@Component({
  selector: 'app-resource',
  templateUrl: './resource.component.html'
})
export class ResourceComponent implements OnInit {
  
  Announcementdata: AnnouncementService;
  constructor(private service: AnnouncementService) {

  }
    ngOnInit() {
       
    }
}