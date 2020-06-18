using System;
using System.Web;
using System.Data;
using System.Diagnostics;
using System.ServiceModel;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

using SSOWebService;
using System.ComponentModel.DataAnnotations;

namespace ChapterPortal.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ChapterOfficerEditModel : PageModel
    {
        [BindProperty]
        public string FName
        {
            get; set;
        }

        [BindProperty]
        public string MName
        {
            get; set;
        }

        [BindProperty]
        public string LName
        {
            get; set;
        }

        [BindProperty]
        public string Name
        {
            get; set;
        }

        [BindProperty]
        public string Description
        {
            get; set;
        }

        [BindProperty]
        public string BeginDate
        {
            get; set;
        }

        [BindProperty]
        public string EndDate
        {
            get; set;
        }

        [BindProperty]
        public string Status
        {
            get; set;
        }

        [BindProperty]
        public string EmailAddress
        {
            get; set;
        }

        [BindProperty]
        public string Phone
        {
            get; set;
        }

        [BindProperty]
        [DataType(DataType.MultilineText)]
        public string Comment
        {
            get; set;
        }

        public DataTable OfficerInfo;

        private readonly ILogger<ChapterOfficerEditModel> _logger;
        public IConfiguration Configuration { get; }

        public ChapterOfficerEditModel(ILogger<ChapterOfficerEditModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            Configuration = configuration;
        }

        private void Refresh()
        {
            Name = HttpContext.Session.GetString("FullName");
            Description = HttpContext.Session.GetString("Description");
            BeginDate = HttpContext.Session.GetString("BeginDate");
            EndDate = HttpContext.Session.GetString("EndDate");
            Status = HttpContext.Session.GetString("Status");
            EmailAddress = HttpContext.Session.GetString("Email");
            Phone = HttpContext.Session.GetString("Phone");
            Comment = HttpContext.Session.GetString("Comment");
        }

        public void OnGet()
        {
            Refresh();
        }

        public void OnPostReset()
        {
            Refresh();
        }

        public void OnPostUpdate()
        {
            string name = Request.Form["name"];
            string description = Request.Form["description"];
            string beginDate = Request.Form["beginDate"];
            string endDate = Request.Form["endDate"];
            string status = Request.Form["status"];
            string email = Request.Form["email"];
            string phone = Request.Form["phone"];
            string comment = Request.Form["comment"];

            string body = "<table><tr><td>Name:&nbsp;&nbsp;</td><td>" + name + "</td></tr>" +
                "<tr><td>Position Description:&nbsp;&nbsp;</td><td>" + HttpUtility.HtmlEncode(description) + "</td></tr>" +
                "<tr><td>Position Begin Date:&nbsp;&nbsp;</td><td>" + beginDate + "</td></tr>" +
                "<tr><td>Position End Date:&nbsp;&nbsp;</td><td>" + endDate + "</td></tr>" +
                "<tr><td>Voting Status:&nbsp;&nbsp;</td><td>" + status + "</td></tr>" +
                "<tr><td>Primary Email Address:&nbsp;&nbsp;</td><td>" + email + "</td></tr>" +
                "<tr><td>Primary Phone:&nbsp;&nbsp;</td><td>" + Phone + "</td></tr>" +
                "<tr><td>Comment:&nbsp;&nbsp;</td><td>" + HttpUtility.HtmlEncode(comment) + "</td></tr></table>";

            // handle the request
            try
            {
                string smtpServer = Configuration.GetSection("MyConfiguration").GetSection("Smtpconfig").GetSection("SMTPServer").Value;
                string smtpDomain = Configuration.GetSection("MyConfiguration").GetSection("Smtpconfig").GetSection("SmtpDomain").Value;
                string smtpLogin = Configuration.GetSection("MyConfiguration").GetSection("Smtpconfig").GetSection("SMTPLogin").Value;
                string smtpPassword = string.Empty; // Configuration.GetSection("MyConfiguration").GetSection("Smtpconfig").GetSection("SMTPPassword").Value;
                string fromEmail = Configuration.GetSection("MyConfiguration").GetSection("Smtpconfig").GetSection("FromEmail").Value;
                string toEmail = Configuration.GetSection("MyConfiguration").GetSection("Smtpconfig").GetSection("MyEmail").Value;
                if (string.IsNullOrEmpty(toEmail))
                    toEmail = Configuration.GetSection("MyConfiguration").GetSection("Smtpconfig").GetSection("AdminEmail").Value;
                else
                    fromEmail = toEmail;
                string emailSender = "Chapter Officer"; //Configuration.GetSection("MyConfiguration").GetSection("Smtpconfig").GetSection("FromDisplayNameEmail").Value;
                string subject = Configuration.GetSection("MyConfiguration").GetSection("Smtpconfig").GetSection("EmailSubject").Value;
                string emailBody = string.Format("<iframe>{0}</iframe>", body);

                EmailModule em = new EmailModule();
                em.SendRegistrationEmail(smtpServer, smtpDomain, smtpLogin, smtpPassword, fromEmail, toEmail, emailSender, subject, emailBody);

                //// successful
                //Name = name;
                //Description = description;
                //BeginDate = beginDate;
                //EndDate = endDate;
                //Status = status;
                //EmailAddress = email;
                //Phone = phone;
                //Comment = comment;
            }
            catch
            {
                throw;
            }
        }
    }
}
