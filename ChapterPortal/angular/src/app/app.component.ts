import { Component } from '@angular/core';
import { LoginComponent } from './login/login.component'
import { AnnouncementService } from './components/announcement.service';
import { CookieService } from 'ngx-cookie-service'
import { Observable } from 'rxjs';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'chapter-portal';
  startdate: string;
  Loginuser: Observable<any>;
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
