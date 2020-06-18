import { HttpClient, HttpEvent, HttpErrorResponse, HttpEventType } from  '@angular/common/http';  
import { map } from  'rxjs/operators';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { AppSettings } from '../shared/classes/AppSettings';
@Injectable({ providedIn: 'root' })
export class CommonService  {
    url: string;
    username: string;
    SERVER_URL: string = "https://file.io/";  
    constructor(private httpClient: HttpClient) {
        //super(_httpService);
    }
    Validateuser(Username:string, pwd:string): Observable<any> {
        this.username = Username;
        this.url = AppSettings.API_EndPoint + `/User/Authenticate/`+Username+'/'+pwd;
        return this.httpClient.get(this.url);
    }

    upload(formData) {

        return this.httpClient.post<any>(this.SERVER_URL, formData, {  
          reportProgress: true,  
          observe: 'events'  
        });  
    }
}