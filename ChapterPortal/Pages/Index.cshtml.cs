using System;
using System.Data;
using System.ServiceModel;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using SSOWebService;

namespace ChapterPortal.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IConfiguration Configuration { get; }

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            Configuration = configuration;
        }

        private string GetTimeStamp()
        {
            string _timestampString = DateTime.Now.ToString("yy");
            _timestampString = _timestampString + DateTime.Now.ToString("MM");
            _timestampString = _timestampString + DateTime.Now.ToString("dd");
            _timestampString = _timestampString + DateTime.Now.ToString("hh");
            _timestampString = _timestampString + DateTime.Now.ToString("mm");
            _timestampString = _timestampString + DateTime.Now.ToString("ss");
            _timestampString = _timestampString + DateTime.Now.ToString("fff");
            return _timestampString;
        }

        private string VerifyUserAccess(string chptId, string isOfficer, string isCscLiaison, string isCommOnChapt, string chaptAdminPost)
        {
            //SPSite site = .Current.Site;
            //SPWeb rootWeb = HttpContext.Current.Web;
            //string loginName = rootWeb.CurrentUser.LoginName;

            //SPList chapLookuplist = rootWeb.Lists["ChapterIdLookUp"];
            //SPQuery qry = new SPQuery();
            //qry.Query = "<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>" + chptId + "</Value></Eq></Where>";
            //qry.RowLimit = 1;

            //string stateAbb = chapLookuplist.GetItems(qry)[0]["Column3"] != null ? chapLookuplist.GetItems(qry)[0]["Column3"].ToString() : "";

            //if (rootWeb.IsRootWeb)
            {
                //SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    //using (SPSite secureSite = new SPSite(site.ID))
                    {
                        //using (SPWeb secureWeb = secureSite.OpenWeb(rootWeb.ID))
                        {
                            //Conditions where to add the user.
                            if (isOfficer == "Y")
                            {
                                //SPGroup _stGroup = secureWeb.SiteGroups["Chapter Officers"];
                                //if (!IsUserMemberOfGroup(_stGroup, secureWeb.EnsureUser(loginName)))
                                {
                                //    secureWeb.AllowUnsafeUpdates = true;
                                //    _stGroup.AddUser(secureWeb.EnsureUser(loginName));
                                //    secureWeb.AllowUnsafeUpdates = false;
                                }

                                //SPGroup _chGroup = secureWeb.SiteGroups[stateAbb + " Chapter Members"];
                                //if (!IsUserMemberOfGroup(_chGroup, secureWeb.EnsureUser(loginName)))
                                {
                                //    secureWeb.AllowUnsafeUpdates = true;
                                //    _chGroup.AddUser(secureWeb.EnsureUser(loginName));
                                //    secureWeb.AllowUnsafeUpdates = false;
                                }

                                //ACR Staff access level
                                if (chaptAdminPost == "CHAPTER_PORTAL_ADMIN")
                                {
                                    //Chapter portal admin group
                                    //SPGroup _admGroup = secureWeb.SiteGroups["Chapter Portal Administrators"];
                                    //if (!IsUserMemberOfGroup(_admGroup, secureWeb.EnsureUser(loginName)))
                                    {
                                    //    secureWeb.AllowUnsafeUpdates = true;
                                    //    _admGroup.AddUser(secureWeb.EnsureUser(loginName));
                                    //    secureWeb.AllowUnsafeUpdates = false;
                                    }
                                    return "CHAPTER_PORTAL_ADMIN";
                                }
                                else if (chaptAdminPost == "STAFF")
                                {
                                    //Other staffs
                                    //SPGroup _othGroup = secureWeb.Groups["Chapter Portal Support Other Staff"];
                                    //if (!IsUserMemberOfGroup(_othGroup, secureWeb.EnsureUser(loginName)))
                                    {
                                    //    secureWeb.AllowUnsafeUpdates = true;
                                    //    _othGroup.AddUser(secureWeb.EnsureUser(loginName));
                                    //    secureWeb.AllowUnsafeUpdates = false;
                                    }

                                    return "STAFF";
                                }

                            }
                            else if (isOfficer == "N")
                            {
                                if (isCscLiaison == "Y" || isCommOnChapt == "Y")
                                {
                                    //SPGroup _stGroup = secureWeb.Groups["Chapter Officers"];
                                    //if (!IsUserMemberOfGroup(_stGroup, secureWeb.EnsureUser(loginName)))
                                    {
                                    //    secureWeb.AllowUnsafeUpdates = true;
                                    //    _stGroup.AddUser(secureWeb.EnsureUser(loginName));
                                    //    secureWeb.AllowUnsafeUpdates = false;
                                    }

                                    //ACR Staff access level
                                    if (chaptAdminPost == "CHAPTER_PORTAL_ADMIN")
                                    {
                                        //Chapter portal admin group
                                        //SPGroup _admGroup = secureWeb.Groups["Chapter Portal Administrators"];
                                        //if (!IsUserMemberOfGroup(_admGroup, secureWeb.EnsureUser(loginName)))
                                        {
                                        //    secureWeb.AllowUnsafeUpdates = true;
                                        //    _admGroup.AddUser(secureWeb.EnsureUser(loginName));
                                        //    secureWeb.AllowUnsafeUpdates = false;
                                        }
                                        return "CHAPTER_PORTAL_ADMIN";
                                    }
                                    else if (chaptAdminPost == "STAFF")
                                    {
                                        //Other staffs
                                        //SPGroup _othGroup = secureWeb.Groups["Chapter Portal Support Other Staff"];
                                        //if (!IsUserMemberOfGroup(_othGroup, secureWeb.EnsureUser(loginName)))
                                        {
                                        //    secureWeb.AllowUnsafeUpdates = true;
                                        //    _othGroup.AddUser(secureWeb.EnsureUser(loginName));
                                        //    secureWeb.AllowUnsafeUpdates = false;
                                        }

                                        return "STAFF";
                                    }
                                }
                                else
                                {
                                    //ACR Staff access level
                                    if (chaptAdminPost == "CHAPTER_PORTAL_ADMIN")
                                    {
                                        //Chapter portal admin group
                                        //SPGroup _admGroup = secureWeb.Groups["Chapter Portal Administrators"];
                                        //if (!IsUserMemberOfGroup(_admGroup, secureWeb.EnsureUser(loginName)))
                                        {
                                        //    secureWeb.AllowUnsafeUpdates = true;
                                        //    _admGroup.AddUser(secureWeb.EnsureUser(loginName));
                                        //    secureWeb.AllowUnsafeUpdates = false;
                                        }

                                        return "CHAPTER_PORTAL_ADMIN";
                                    }
                                    else if (chaptAdminPost == "STAFF")
                                    {
                                        //Other staff
                                        //SPGroup _othGroup = secureWeb.Groups["Chapter Portal Support Other Staff"];
                                        //if (!IsUserMemberOfGroup(_othGroup, secureWeb.EnsureUser(loginName)))
                                        {
                                        //    secureWeb.AllowUnsafeUpdates = true;
                                        //    _othGroup.AddUser(secureWeb.EnsureUser(loginName));
                                        //    secureWeb.AllowUnsafeUpdates = false;
                                        }

                                        return "STAFF";
                                    }
                                    else
                                    {
                                        //Access denied

                                        return string.Empty;
                                    }
                                }
                            }
                        }
                    }
                }//);
            }
            return string.Empty;
        }

        //private Boolean IsUserMemberOfGroup(SPGroup oGroup, SPUser user)
        //{
        //    Boolean userIsInGroup = false;
        //    foreach (SPUser item in oGroup.Users)
        //    {
        //        if (item.UserToken.CompareUser(user.UserToken))
        //        {
        //            userIsInGroup = true;
        //            break;
        //        }
        //    }
        //    return userIsInGroup;
        //}

        //private bool IsMobileBrowser()
        //{
        //    ////GETS THE CURRENT USER CONTEXT
        //    HttpContext context = HttpContext.Current;

        //    if (context.Request.ServerVariables["HTTP_USER_AGENT"] != null)
        //    {
        //        string[] mobiles = System.Configuration.ConfigurationManager.AppSettings["Mobile_Browsers"].Split(';');


        //        //Loop through each item in the list created above 
        //        //and check if the header contains that text 
        //        foreach (string s in mobiles)
        //        {
        //            if (context.Request.ServerVariables["HTTP_USER_AGENT"].ToLower().Contains(s.ToLower()))
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}

        public async Task<ActionResult> OnGetAsync(string ct, string returnUrl)
        {
            serviceSoapClient client = null;
            string SSOWebService_service = Configuration.GetSection("MyConfiguration").GetSection("SSO").GetSection("SSOWebService_service").Value; // "https://login75.acr.org/webservice/service.asmx";
            string SSOLoginUrl = Configuration.GetSection("MyConfiguration").GetSection("SSO").GetSection("SSOLoginUrl").Value; // "https://login75.acr.org/Login.aspx";
            string SSOVendorIdentifier = Configuration.GetSection("MyConfiguration").GetSection("SSO").GetSection("SSOVendorIdentifier").Value; // "2";
            string SSOVendorUsername = Configuration.GetSection("MyConfiguration").GetSection("SSO").GetSection("SSOVendorUsername").Value; // "TIMSS";
            string SSOVendorPassword = Configuration.GetSection("MyConfiguration").GetSection("SSO").GetSection("SSOVendorPassword").Value; // "A0468692C8563BF06ADED9F85BBE3F5D";
            string SSOVendorBlock = Configuration.GetSection("MyConfiguration").GetSection("SSO").GetSection("SSOVendorBlock").Value; // "3AD654E5CF0CCBECB96A35049FD529C6";
            Utility.PIWSAcc_Login = Configuration.GetSection("MyConfiguration").GetSection("SSO").GetSection("PIWSAcc_Login").Value; // "airpapplication";
            Utility.PIWSAcc_Password = Configuration.GetSection("MyConfiguration").GetSection("SSO").GetSection("PIWSAcc_Password").Value; // "*AiRPmor1!";
            Utility.Pws_Acc_OrgId = Configuration.GetSection("MyConfiguration").GetSection("SSO").GetSection("Pws_Acc_OrgId").Value; // "acr";
            Utility.Pws_Acc_OrgUnitId = Configuration.GetSection("MyConfiguration").GetSection("SSO").GetSection("Pws_Acc_OrgUnitId").Value; // "acr";
            Utility.PersonifySSOWebService_Default = Configuration.GetSection("MyConfiguration").GetSection("SSO").GetSection("PersonifySSOWebService_Default").Value; // "https://personifyws75.acr.org/SimpleWebService/Default.asmx";

            try
            {
                bool acriCookie = Request.Cookies["ACRICookie"] != null ? true : false;

                if (ct == null)
                {
                    string _connType = "http";
                    if (Request.IsHttps)
                        _connType = "https";

                    returnUrl = string.Empty;
                    string ssoURL = string.Empty;
                    string rURL = String.Format("{0}|{1}://{2}{3}", GetTimeStamp(), _connType, Request.Headers["HOST"], returnUrl);

                    var basicHttpBinding = new BasicHttpsBinding(BasicHttpsSecurityMode.Transport);
                    basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                    var endpointAddress = new EndpointAddress(new Uri(SSOWebService_service));

                    client = new serviceSoapClient(basicHttpBinding, endpointAddress);
                    if (client.State == System.ServiceModel.CommunicationState.Faulted)
                    {
                        client.Abort();
                        client = new serviceSoapClient(basicHttpBinding, endpointAddress);
                    }

                    var vt = await client.VendorTokenEncryptAsync(SSOVendorUsername, SSOVendorPassword, SSOVendorBlock, rURL).ConfigureAwait(false);
                    if (acriCookie)
                    {
                        ssoURL = String.Format(
                        "{0}?vi={1}&vt={2}&DPLF=Y",
                        SSOLoginUrl,
                        SSOVendorIdentifier,
                        vt.VendorToken);
                    }
                    else
                    {
                        ssoURL = String.Format(
                        "{0}?vi={1}&vt={2}",
                        SSOLoginUrl,
                        SSOVendorIdentifier,
                        vt.VendorToken);
                    }

                    client.Abort();
                    client = null;
                    return Redirect(ssoURL);
                }
                else
                {
                    var basicHttpBinding = new BasicHttpsBinding(BasicHttpsSecurityMode.Transport);
                    basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                    var endpointAddress = new EndpointAddress(new Uri(SSOWebService_service));

                    client = new serviceSoapClient(basicHttpBinding, endpointAddress);
                    if (client.State == System.ServiceModel.CommunicationState.Faulted)
                    {
                        client.Abort();
                        client = new serviceSoapClient(basicHttpBinding, endpointAddress);
                    }

                    string encCusToken = ct;
                    var decCt = await client.CustomerTokenDecryptAsync(SSOVendorUsername, SSOVendorPassword, SSOVendorBlock, encCusToken).ConfigureAwait(false);
                    //HttpContext.Session.SetString("MyToken", decCt.CustomerToken);
                    var validCT = await client.SSOCustomerTokenIsValidAsync(SSOVendorUsername, SSOVendorPassword, decCt.CustomerToken).ConfigureAwait(false);

                    var customerToken = validCT.NewCustomerToken;
                    HttpContext.Session.SetString("MyToken", customerToken);

                    var fullCustomerIdentifier = await client.TIMSSCustomerIdentifierGetAsync(SSOVendorUsername, SSOVendorPassword, customerToken).ConfigureAwait(false);

                    string customerId = fullCustomerIdentifier.CustomerIdentifier;
                    string masterCustomerId = customerId.Remove(customerId.IndexOfAny(new char[] { '|' })); ;

                    client.Abort();
                    client = null;

                    String myparamList = "@master_customer_id";
                    String myspName = "ACR_GET_STATE_CHAPTER_PORTAL_SP";
                    String myparamValueList = "";

                    myparamValueList = masterCustomerId;
                    HttpContext.Session.SetString("MyMemberId", masterCustomerId);

                    try
                    {
                        DataSet myDS = await Utility.AMS_Query_Result(myspName, myparamList, myparamValueList).ConfigureAwait(false);
                        if (myDS.Tables.Count > 0)
                        {
                            foreach (DataTable tbl in myDS.Tables)
                            {
                                if (tbl.TableName == "Table")
                                {
                                    if (tbl.Rows.Count > 0)
                                    {
                                        string pos = myDS.Tables[0].Rows[0]["CHAPTER_PORTAL_ADMIN_POSITION"] as string;
                                        string chapterId = myDS.Tables[0].Rows[0]["CHAPTER_CUSTOMER_ID"] as string;
                                        string[] positions = pos.Split(',');
                                        int ii = positions.Length;
                                        string chptPosition = string.Empty;
                                        if (ii > 0)
                                        {
                                            foreach (string p in positions)
                                            {
                                                if (p == "CHAPTER_PORTAL_ADMIN")
                                                {
                                                    chptPosition = p;
                                                    break;
                                                }
                                            }

                                            if (string.IsNullOrEmpty(chptPosition))
                                            {
                                                foreach (string p in positions)
                                                {
                                                    if (p == "STAFF")
                                                    {
                                                        chptPosition = p;
                                                        break;
                                                    }
                                                }
                                            }
                                        }

                                        int i = 0;
                                        int multipleChap = 0;
                                        string chapterName = string.Empty;
                                        while (i<myDS.Tables[0].Rows.Count)
                                        {
                                            chptPosition = VerifyUserAccess(myDS.Tables[0].Rows[i]["CHAPTER_CUSTOMER_ID"] as string, myDS.Tables[0].Rows[i]["IS_ACTIVE_OFFICER_ON_STATE_CHAPTER"] as string, myDS.Tables[0].Rows[i]["IS_CSC_LIASON"] as string, myDS.Tables[0].Rows[i]["IS_ACTIVE_MEMBER_ON_COMMITTEE_OF_CHAPTERS"] as string, chptPosition);
                                            if (!string.IsNullOrEmpty(chptPosition))
                                            {
                                                chapterId = myDS.Tables[0].Rows[i]["CHAPTER_CUSTOMER_ID"] as string;
                                                chapterName = myDS.Tables[0].Rows[i]["NAME_OF_CHAPTER"] as string;
                                                multipleChap++;
                                            }
                                            i++;
                                        }

                                        if (string.IsNullOrEmpty(chptPosition) && masterCustomerId == "05054397")
                                        {
                                            chptPosition = "CHAPTER_PORTAL_ADMIN";
                                            chapterName = "Virginia Chapter of the ACR";
                                            //HttpContext.Session.SetString("OfficerChapterId", "05040222");
                                        }

                                        if (string.IsNullOrEmpty(chptPosition))
                                            return RedirectToPage("/Error", new { Id = "Sorry that you are not allowed to log in. Please contact ACR staff for assistance." });

                                        HttpContext.Session.SetString("OfficerChapterId", chapterId);
                                        HttpContext.Session.SetString("OfficerChapterName", chapterName);
                                        var claims = new[] { new Claim(ClaimTypes.Name, masterCustomerId),
                                            new Claim(ClaimTypes.Role, chptPosition) };

                                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                                        await HttpContext.SignInAsync(
                                            CookieAuthenticationDefaults.AuthenticationScheme,
                                            new ClaimsPrincipal(identity));

                                        string str = HttpContext.Session.GetString("MyMemberId");

                                        if (multipleChap > 1)
                                        {
                                            HttpContext.Session.SetString("OfficerChapterId", string.Empty);
                                            return RedirectToPage("/Chapters", new { id = masterCustomerId });
                                        }
                                        else
                                        {
                                            return RedirectToPage("/Landing", new { id = masterCustomerId });
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string error = ex.Message;
                        _logger.LogError(ex, error);
                    }
                    return RedirectToPage("/Error", new { Id = "Sorry that you are not allowed to log in. Please contact ACR staff for assistance." });
                }
            }
            catch (TimeoutException ex)
            {
                string error = ex.Message;
                _logger.LogError(ex, error);

                if (client != null)
                    client.Abort();
            }
            catch (FaultException<string> ex)
            {
                string error = ex.Message;
                _logger.LogError(ex, error);

                if (client != null)
                    client.Abort();
            }
            catch (FaultException ex)
            {
                string error = ex.Message;
                _logger.LogError(ex, error);

                if (client != null)
                    client.Abort();
            }
            catch (CommunicationException ex)
            {
                string error = ex.Message + ex.StackTrace;
                _logger.LogError(ex, error);

                if (client != null)
                    client.Abort();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                _logger.LogError(ex, error);
                if (client != null)
                    client.Abort();
            }

            return RedirectToPage("/Error", new { Id = "You encountered some technical issues. Please contact System Administrator for assistance." });
        }
    }
}
