declare var siteName: any;
export class AppSettings {
   // public static SiteName = 'Chapter Porta;';
    public static App_Host = window.location.protocol + '//' + window.location.host + '/';
    public static API_EndPoint = 'https:' + '//' + 'localhost:44310';
   // public static MVC_Endpoint = window.location.protocol + '//' + window.location.host + '/' + AppSettings.SiteName + '/';
    public static App_Version = 'Chapter Portal version';
    // public static ImageHandlerUrl = `${window.location.protocol}//${window.location.host}/ImageHandler/ShowHandler.ashx?location=`;
    // public static CiPDEWUrl =`${window.location.protocol}//${window.location.host}/CaseSubmission/cipdew?id=`;

}