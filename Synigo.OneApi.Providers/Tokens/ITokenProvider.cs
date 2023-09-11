using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synigo.OneApi.Providers.Tokens
{
    public interface ITokenProvider
    {
        Task<string> GetSynigoApplicationTokenAsync();
    }
}
