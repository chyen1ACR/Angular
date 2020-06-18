import { Component, OnInit } from '@angular/core';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker/public_api';
import { AnnouncementService } from '../announcement.service'
import { CookieService } from 'ngx-cookie-service';
import { Router, ActivatedRoute } from '@angular/router';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
import { ChapterOfficer} from '../../shared/model/Announcement.model'
import { AnnouncementModule } from '../announcement.module';
declare var tinymce: any;
@Component({
  selector: 'app-chapter-officer',
  templateUrl: './chapterOfficeEdit.component.html'
})
export class chapterOfficeEditComponent implements OnInit {

  LoginUser: any;
  ChapterOfficerDetails:ChapterOfficer;
Name:string;
Description: string;
BeginDate: string;
EndDate:string;
Status:string;
EmailAddress:string;
Phone:string;
Comments:string;
ValidationMessage: any = {
    type: 'Success',
    MessageText: ''
};
  constructor(private modalService: NgbModal, private route: Router, private service: AnnouncementService) {
   
    this.service.getSectionSubject().subscribe((res) => {this.LoginUser = res; });
  }

  ngOnInit() {
  }

  SaveOfficer(){
  this.ChapterOfficerDetails = new ChapterOfficer();
  this.ChapterOfficerDetails.CustomerId = this.LoginUser.userName;
  this.ChapterOfficerDetails.FullName = this.Name;
  this.ChapterOfficerDetails.PositionDescription = this.Description;
  this.ChapterOfficerDetails.PositionBeginDate = this.BeginDate;
  this.ChapterOfficerDetails.PositionEndDate = this.EndDate;
  this.ChapterOfficerDetails.VotingStatus = this.Status;
  this.ChapterOfficerDetails.PrimaryEmail = this.EmailAddress;
  this.ChapterOfficerDetails.PrimaryPhone = this.Phone;
  this.ChapterOfficerDetails.AdditionalComments = this.Comments;
  this.service.UpdateChapterOfficeDetails(this.ChapterOfficerDetails).subscribe((res)=>{
      if(res){
    this.ValidationMessage.MessageText ='Officer details updated successfully.';
    this.ValidationMessage.type = 'Success';
    this.ResponseMessage();
      }
      else{
        this.ValidationMessage.MessageText ='Not Updated';
        this.ValidationMessage.type = 'Error';
        this.ResponseMessage();
      }
  });
  }

  Clear(){
    this.Name = "";
    this.Description = "";
    this.BeginDate = "";
    this.EndDate = "";
    this.Status = "";
    this.EmailAddress = "";
    this.Phone = "";
    this.Comments = "";
    
  }

  GoBackWindow(){
      this.route.navigate(['/']);
  }
  ResponseMessage() {
    setTimeout(() => {
        this.ValidationMessage.MessageText = '';
    }, 3000);
}

}
