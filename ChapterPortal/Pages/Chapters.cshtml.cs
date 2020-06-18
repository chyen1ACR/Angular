using System;
using System.IO;
using System.Data;
using System.Drawing;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;

using OfficeOpenXml;
using PdfRpt.Core.Contracts;
//using PdfRpt.Core.FunctionalTests.Models;
using PdfRpt.FluentInterface;
using Aspose.Cells;

namespace ChapterPortal.Pages
{
    public class Chapter
    {
        public string Name { get; set; }
        public string ChapterId { get; set; }

        public Chapter(string name, string chapterId)
        {
            Name = name;
            ChapterId = chapterId;
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ChaptersModel : PageModel
    {
        public IList<Chapter> myChapters;

        private readonly ILogger<ChaptersModel> _logger;
        public IConfiguration Configuration { get; }

        public ChaptersModel(ILogger<ChaptersModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            Configuration = configuration;
        }
        public ActionResult OnPostChapter(string id)
        {
            string[] chapter = id.Split('|');
            if (chapter.Length == 2)
            {
                HttpContext.Session.SetString("OfficerChapterId", chapter[0]);
                HttpContext.Session.SetString("OfficerChapterName", chapter[1]);
                string str = HttpContext.Session.GetString("MyMemberId");
                return RedirectToPage("/Landing", new { Id = str });
            }
            else return null;
        }

        public async Task<ActionResult> OnGetAsync(string id)
        {
            string chapterId = HttpContext.Session.GetString("OfficerChapterId");
            string str = HttpContext.Session.GetString("MyMemberId");
            if (!string.IsNullOrEmpty(chapterId) && !string.IsNullOrEmpty(id))
            {
                return RedirectToPage("/Landing", new { id = str });
            }

            String myparamList = "@master_customer_id";
            String myspName = "ACR_GET_STATE_CHAPTER_PORTAL_SP";
            String myparamValueList = "";

            myparamValueList = HttpContext.Session.GetString("MyMemberId");

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

                                myChapters = new List<Chapter>();
                                int i = 0;
                                while (i < myDS.Tables[0].Rows.Count)
                                {
                                    chptPosition = VerifyUserAccess(myDS.Tables[0].Rows[i]["CHAPTER_CUSTOMER_ID"] as string, myDS.Tables[0].Rows[i]["IS_ACTIVE_OFFICER_ON_STATE_CHAPTER"] as string, myDS.Tables[0].Rows[i]["IS_CSC_LIASON"] as string, myDS.Tables[0].Rows[i]["IS_ACTIVE_MEMBER_ON_COMMITTEE_OF_CHAPTERS"] as string, chptPosition);
                                    if (!string.IsNullOrEmpty(chptPosition))
                                    {
                                        Chapter item = new Chapter(myDS.Tables[0].Rows[i]["NAME_OF_CHAPTER"] as string, myDS.Tables[0].Rows[i]["CHAPTER_CUSTOMER_ID"] as string);
                                        myChapters.Add(item);
                                    }
                                    i++;
                                }
                            }
                        }
                    }
                }
            }
            catch { }

            //if (myChapters == null)
            //{
            //    myChapters = new List<Chapter>();
            //}
            //if (myChapters.Count<1)
            //{
            //    Chapter item = new Chapter("American College of Radiology", "05040222");
            //    myChapters.Add(item);
            //    item = new Chapter("Virginia Association of Radiology", "05040222");
            //    myChapters.Add(item);
            //}
            return null;
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
    }
}
