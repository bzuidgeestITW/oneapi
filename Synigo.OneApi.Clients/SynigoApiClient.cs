using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Synigo.OneApi.Providers.Tokens;
using System.Net.Http;
using System.Threading.Tasks;

namespace Synigo.OneApi.Clients
{
    public class SynigoApiClient
    {
        private static HttpClient _httpClient = new HttpClient();

        private readonly ITokenProvider _tokenProvider;

        public SynigoApiClient(IConfiguration configuration, ITokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            request = await AuthorizeRequest(request);
            return await _httpClient.SendAsync(request);
        }

        private async Task<HttpRequestMessage> AuthorizeRequest(HttpRequestMessage request)
        {
            request.Headers.Add("Authorization", "Bearer " + await _tokenProvider.GetSynigoApplicationTokenAsync());
            return request;
        }

    }
}
