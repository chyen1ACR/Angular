using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SSOWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChapterPortal.Connected_Services.SSOWebService
{
    public class PersonifyHelperservice
    {
        private serviceSoapClient _ssoService;
        public IConfiguration Configuration { get; }
        public string SSOVendorUsername = string.Empty;
        public string SSOVendorPassword = string.Empty;
        public PersonifyHelperservice(IConfiguration configuration)
        {
            Configuration = configuration;
           SSOVendorUsername = Configuration.GetSection("MyConfiguration").GetSection("SSO").GetSection("SSOVendorUsername").Value; // "TIMSS";
           SSOVendorPassword = Configuration.GetSection("MyConfiguration").GetSection("SSO").GetSection("SSOVendorPassword").Value; // "A0468692C8563BF06ADED9F85BBE3F5D";

    }
    public serviceSoapClient SSOServiceProxy
        {

            get
            {
                if (_ssoService == null)
                {
                    serviceSoapClient.EndpointConfiguration endpoint = new serviceSoapClient.EndpointConfiguration();
                    _ssoService = new serviceSoapClient(endpoint);
                }
                return _ssoService;
            }
        }
        //public IConfigurationRoot Configuration => AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder(), addUserSecrets: true);
        //IConfigurationRoot _appConfiguration = AppConfigurations.Get(Directory.GetCurrentDirectory());
        public bool Authenticate(string UserName, string Password)
        {
            var oResult = SSOServiceProxy.SSOCustomerAuthenticateAsync(SSOVendorUsername, SSOVendorPassword, UserName, Password).Result;
            return oResult.Result;
        }

        public dynamic GetPersonifyCUstomer(string UserName)
        {
            var oResult = SSOServiceProxy.SSOCustomerGetByUsernameAsync(SSOVendorUsername, SSOVendorPassword, UserName);
            return oResult;
        }
    }
}
