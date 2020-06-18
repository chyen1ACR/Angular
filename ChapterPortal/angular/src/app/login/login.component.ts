import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import {CommonService} from '../services/common.service'
import {CookieService} from 'ngx-cookie-service'
import { AnnouncementService } from '../components/announcement.service'

@Component({ templateUrl: 'login.component.html' })
export class LoginComponent implements OnInit {
    loading = false;
    submitted = false;
    returnUrl: string;
    userid : string = "";
    IsvalidUser:boolean=true;
    IsCurrentUserValid:boolean = false;
    IsuserNameEmpty:boolean= false;
    IsPasswordEmpty:boolean = false;
    pwd:string="";
    constructor(
        private route: ActivatedRoute,
        private router: Router,private service:CommonService, private cookieService:CookieService,private announcementservice: AnnouncementService) {
          if(localStorage.getItem("userid")!= null){
            this.IsvalidUser = true;
          }
    }

    ngOnInit() {
        // get return url from route parameters or default to '/'
        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
    }


    onSubmit() {
      if(this.IsValid())
      this.service.Validateuser(this.userid,this.pwd).subscribe((res) => {
      if(res){
          this.announcementservice.GetCustomerdata(this.userid).subscribe((res) => {        
            this.announcementservice.setSectionSubject(res.result);
        const dateNow = new Date();
        dateNow.setMinutes(dateNow.getMinutes() + 60);
        this.cookieService.set('userid',this.userid,dateNow);
        this.IsvalidUser= true;
        this.router.navigate([this.returnUrl]);
        
        });        
      }
      else{
        this.IsvalidUser= false;
        this.router.navigate([this.returnUrl]);
      }
      });  
    }
   

    IsValid(){
      if(this.userid != "" && this.pwd != "")
          return true;
      else {
       this.IsuserNameEmpty = this.userid == "";
       this.IsPasswordEmpty = this.pwd == "";
       return false;
      }
    }

   
}