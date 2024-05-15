using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Services
{
    public  interface IResponseCashService
    {
        // cash Data
        Task CacheResponseAsync(string Cashkey, object Response, TimeSpan ExpireTime);

        // Get Cashed Data
        Task<string?> GetCacheResponse(string Cashkey);  
    }
}
