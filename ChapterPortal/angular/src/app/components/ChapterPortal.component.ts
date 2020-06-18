import { Component, OnInit } from '@angular/core';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker/public_api';
import { CookieService } from 'ngx-cookie-service';
import { AnnouncementService } from '../components/announcement.service';
import { Router, ActivatedRoute } from '@angular/router';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
@Component({
  selector: 'app-ChapterPortal',
  templateUrl: './ChapterPortal.component.html'
})
export class ChapterPortalComponent implements OnInit {
    title = 'chapter-portal';
  startdate: string;
  Loginuser: any;
  UserName: any;
  Announcementdata: AnnouncementService;
  constructor(private service: AnnouncementService, private cookieService: CookieService) {

    this.service.getSectionSubject().subscribe((res) => {
      this.Loginuser = res;
      this.UserName = this.cookieService.get('userid');
    });
  }
    ngOnInit() {
        if (this.UserName != "") {
          this.service.GetCustomerdata(this.UserName).subscribe((res) => {
            this.service.setSectionSubject(res.result);
          });
        }
    }
}