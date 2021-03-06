using System;
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

namespace ChapterPortal.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class LandingModel : PageModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        private readonly ILogger<LandingModel> _logger;
        public IConfiguration Configuration { get; }

        public LandingModel(ILogger<LandingModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            Configuration = configuration;
        }

        public void OnGet(string id)
        {
            ViewData["Id"] = id;
            ViewData["ChapterId"] = HttpContext.Session.GetString("OfficerChapterId");
            ViewData["ChapterName"] = HttpContext.Session.GetString("OfficerChapterName");

            //ViewData["Id"] = HttpContext.Session.GetString("MyMemberId");
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }

        protected async void SignOut()
        {
            serviceSoapClient client = null;
            try
            {
                string ctoken = HttpContext.Session.GetString("MyToken");
                string SSOVendorUsername = Configuration.GetSection("MyConfiguration").GetSection("SSO").GetSection("SSOVendorUsername").Value; // "TIMSS";
                string SSOVendorPassword = Configuration.GetSection("MyConfiguration").GetSection("SSO").GetSection("SSOVendorPassword").Value; // "A0468692C8563BF06ADED9F85BBE3F5D";
                string SSOWebService_service = Configuration.GetSection("MyConfiguration").GetSection("SSO").GetSection("SSOWebService_service").Value; // "https://login75.acr.org/webservice/service.asmx";

                var basicHttpBinding = new BasicHttpsBinding(BasicHttpsSecurityMode.Transport);
                basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                var endpointAddress = new EndpointAddress(new Uri(SSOWebService_service));

                client = new serviceSoapClient(basicHttpBinding, endpointAddress);
                if (client.State == System.ServiceModel.CommunicationState.Faulted)
                {
                    client.Abort();
                    client = new serviceSoapClient(basicHttpBinding, endpointAddress);
                }

                var decCt = await client.SSOCustomerLogoutAsync(SSOVendorUsername, SSOVendorPassword, ctoken).ConfigureAwait(false);
                client.Abort();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                _logger.LogError(ex, error);
                if (client != null)
                    client.Abort();
            }
        }

        public async Task<ActionResult> OnPostLogOff()
        {
            SignOut();
            await HttpContext.SignOutAsync();
            return Redirect("/Index");
        }
    }
}
