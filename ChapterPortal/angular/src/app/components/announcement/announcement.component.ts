import { Component, OnInit } from '@angular/core';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker/public_api';
import { AnnouncementService } from '../announcement.service'
import { CookieService } from 'ngx-cookie-service';
import { Router, ActivatedRoute } from '@angular/router';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
import {Announcementmodel} from '../../shared/model/Announcement.model'
import { AnnouncementModule } from '../announcement.module';
declare var tinymce: any;
@Component({
  selector: 'app-announcement',
  templateUrl: './announcement.component.html'
})
export class AnnouncementComponent implements OnInit {

  bsInlineValue = new Date();
  bsInlineRangeValue: Date[];
  maxDate = new Date();
  colorTheme = 'theme-dark-blue';
  minDate: Date;
  daterange:any;
  bsConfig: Partial<BsDatepickerConfig>;
  LoginUser: any;
  announcements: any;
  dataModel: string;
  dropdownList: any;
  selectedItems: Array<any> = [];
  dropdownSettings: any = {};
  CaptersList: any;
  UserName: string = "";
  closeResult: string;
  newannouncement: Announcementmodel;
  ValidationMessage: any = {
    type: 'Success',
    MessageText: ''
};
  constructor(private modalService: NgbModal, private route: Router, private service: AnnouncementService, private cookiService: CookieService) {
    this.maxDate.setDate(this.maxDate.getDate() + 7);
    this.bsInlineRangeValue = [this.bsInlineValue, this.maxDate];
    this.bsConfig = Object.assign({}, { containerClass: this.colorTheme, isAnimated: true });
    this.minDate = new Date();
  }

  ngOnInit() {
    this.UserName = this.cookiService.get('userid');
    this.service.GetCustomerdata(this.UserName).subscribe((res) => {
      this.LoginUser = res.result;
      this.service.setSectionSubject(this.LoginUser);
      console.log(res);
      this.dropdownList = [];
      this.service.getChaptersinfo().subscribe((res) => {
        this.dropdownList = res;
        this.dropdownSettings = {
          singleSelection: false,
          idField: 'chapterId',
          textField: 'name',
          selectAllText: 'Select All',
          unSelectAllText: 'UnSelect All',
          itemsShowLimit: 1,
          allowSearchFilter: true
        };
        this.service.GetAnnouncements(this.dropdownList).subscribe((result) => {
          if(result.length%2 == 1){
            result[result.length-1].IsOddOne = true;
          }
          this.announcements = result;
        });
      });
    });
  }

  open(content) {
    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title', windowClass: 'SuccessLogOut', size: 'lg', backdrop: 'static', centered: true }).result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });
  }

  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return `with: ${reason}`;
    }
  }

  saveAnnouncement() {    
    this.newannouncement = new Announcementmodel();
      this.newannouncement.PlainText= tinymce.editors[0].getContent({ format: 'text' }).trim(),
      this.newannouncement.Text = tinymce.editors[0].getContent().trim(),
      this.newannouncement.CreatedBy = parseInt(this.LoginUser.userName),
      this.newannouncement.StartDate = this.daterange[0].toDateString(),
      this.newannouncement.EndDate = this.daterange[1].toDateString(),
      this.newannouncement.ModifiedBy = parseInt(this.LoginUser.userName),
      this.newannouncement.ChpaterId = this.selectedItems;
    this.service.saveannouncement(this.newannouncement).subscribe((reslut) => {      
      if(reslut){
        this.service.GetAnnouncements(this.dropdownList).subscribe((res) => {
          if(res.length%2 == 1){
            res[res.length-1].IsOddOne = true;
          }
          this.announcements = res;
        });
      this.ValidationMessage.MessageText ='Officer details updated successfully.';
      this.ValidationMessage.type = 'Success';
      this.ResponseMessage();
        }
        else{
          this.ValidationMessage.MessageText ='Not Updated';
          this.ValidationMessage.type = 'Error';
          this.ResponseMessage();
        }
      console.log(reslut);
    });
  }

  DeleteAnnouncement(params: any) {
    var parms = {
      Id: params.id,
      PlainText: tinymce.editors[0].getContent({ format: 'text' }).trim(),
      Text: tinymce.editors[0].getContent().trim(),
      CreatedBy: 1,
      StartDate: this.bsInlineRangeValue[0].toDateString(),
      EndDate: this.bsInlineRangeValue[1].toDateString(),
      ModifiedBy: 1,
    }
    this.service.DeleteAnnouncement(parms).subscribe((res) => {
      this.announcements = res;
    })
  }
  onItemSelect(item: any) {
    console.log(item);
  }
  onSelectAll(items: any) {
    console.log(items);
  }
  ResponseMessage() {
    setTimeout(() => {
        this.ValidationMessage.MessageText = '';
    }, 3000);
}
saveAn(){
  debugger
}
}
