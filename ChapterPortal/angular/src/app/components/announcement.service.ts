import { Injectable } from '@angular/core';
import { HttpClient, HttpEvent, HttpErrorResponse, HttpEventType, HttpHeaders } from  '@angular/common/http';  
import { map } from  'rxjs/operators';
import { BehaviorSubject, Observable } from 'rxjs';
import { AppSettings } from '../shared/classes/AppSettings';

@Injectable()
export class AnnouncementService {
   
    SERVER_URL: string = "https://file.io/";  
    public LoginUser = new BehaviorSubject<any>('');  
    public url: string;
    public username:string;
    public ChapterInfo = new  BehaviorSubject<any>('');
    constructor(private http: HttpClient) {
        //super(_httpService);
    }
    Savefile({ File, FolderId,IsFolderUpload }: { File: any; FolderId: number;IsFolderUpload:boolean; }): Observable<any> {
        let formData: FormData = new FormData();
        formData.append(File.webkitRelativePath, File, File.name);
        let headers = new HttpHeaders({
            'Accept': 'application/json',
            'X-FILE-NAME': File.name
        });
        //console.log(headers.get('X-FILE-NAME'));
        let options = { headers: headers };
        return this.http.post(AppSettings.API_EndPoint + `/Home/FileUpload/${FolderId}/${IsFolderUpload}`, formData,options).pipe(
          map(Response => Response)
        )
      }
      GetFolders(){
        this.url = AppSettings.API_EndPoint + `/Home`;
        return this.http.get(this.url)
      }
    //   GetAllFiles(){
    //     this.url = AppSettings.API_EndPoint + `/Home/GetAllFiles`;
    //     return this.http.get(this.url)
    //   }
    upload(formData) {

        return this.http.post<any>(this.SERVER_URL, formData, {  
          reportProgress: true,  
          observe: 'events'  
        });  
    }
    CheckLogin(customerId: string): Observable<any> {
        this.url = AppSettings.API_EndPoint + `Announcement/IsValidCustomer/`+customerId;
        return this.http.get(this.url)
    }
    saveannouncement(parms:any): Observable<any> {
        this.url = AppSettings.API_EndPoint + `/Announcement`;
        return this.http.post(this.url,parms);
    } 
     
    DeleteAnnouncement(params:any): Observable<any> {
        this.url = AppSettings.API_EndPoint + `/Announcement/DeleteAnnouncement`;
        return this.http.post(this.url,params);
    } 
    GetAnnouncements(params): Observable<any> {
        this.url = AppSettings.API_EndPoint + `/Announcement/GetAnnoumncements`;
        return this.http.post(this.url,params);
    }

    GetCustomerdata(UserName:string): Observable<any> {
        this.url = AppSettings.API_EndPoint + `/user/GetCustomerdata/`+UserName;
        return this.http.get(this.url);
    }

    GetCustomerChapters(username:string){
        this.url = AppSettings.API_EndPoint+`/user/GetCustomerChaptersAsync/`+username;
        return this.http.get(this.url);
    }

    getSectionSubject(){
        return this.LoginUser.asObservable();
      }
    
    setSectionSubject(LoginUser) {
        this.LoginUser.next(LoginUser);
      }
    UpdateChapterOfficeDetails(params):Observable<any> {
        this.url = AppSettings.API_EndPoint+`/Chapter/SaveOrUpdateChapteOfficer`;
        return this.http.post(this.url,params);
    }
    setChaptersinfo(chapterinfo){
        this.ChapterInfo.next(chapterinfo);
    }

    getChaptersinfo(){
        return this.ChapterInfo;
    }
}
