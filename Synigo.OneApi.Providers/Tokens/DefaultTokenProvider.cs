using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synigo.OneApi.Providers.Tokens
{
    public class DefaultTokenProvider : ITokenProvider
    {
        private readonly IConfidentialClientApplication _confidentialClientApplication;
        private readonly IConfiguration _configuration;

        public DefaultTokenProvider(IConfidentialClientApplication confidentialClientApplication, IConfiguration configuration)
        {
            _confidentialClientApplication = confidentialClientApplication;
            _configuration = configuration;
        }

        public async Task<string> GetSynigoApplicationTokenAsync()
        {
            var scopes = new string[] { $"{_configuration.GetValue<string>("AzureAD_SynigoResourceId")}/.default" };
            var authResult = await _confidentialClientApplication.AcquireTokenForClient(scopes)
                                                                 .ExecuteAsync();
            return authResult.AccessToken;
        }
    }
}
