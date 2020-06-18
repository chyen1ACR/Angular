import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import {AnnouncementService} from '../../components/announcement.service';

import { CookieService } from 'ngx-cookie-service';
import { Route } from '@angular/compiler/src/core';
@Component({ 
    selector:"<app-header>",
    templateUrl: 'header.component.html' })
export class HeaderComponent implements OnInit {
    Loginuser:any;
    UserName: string;
    dropdownList: Object;
    constructor(private service:AnnouncementService, private cookieservice:CookieService, private route:Router){
        this.UserName = this.cookieservice.get('userid');
       this.service.getSectionSubject().subscribe((res) =>{
           this.Loginuser = res;
           
       });
    }

    ngOnInit() {
        this.service.GetCustomerChapters(this.UserName).subscribe((res) => {
            this.service.setChaptersinfo(res);
            this.dropdownList = res;
          });
       }

       Logout() {
        this.service.setSectionSubject('');
        this.cookieservice.deleteAll();
        this.route.navigate(['/login']);
      }
}