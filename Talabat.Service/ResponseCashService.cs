using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Services;

namespace Talabat.Service
{
    public class ResponseCashService : IResponseCashService
    {
        private readonly IDatabase _database;
        public ResponseCashService(IConnectionMultiplexer Redis )
        {
            _database = Redis.GetDatabase();    
        }
    



        public async Task CacheResponseAsync(string Cashkey, object Response, TimeSpan ExpireTime)
        {
            if (Response is null) return;
                    var Options = new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase

                    };
            var SerializerResponse = JsonSerializer.Serialize(Response, Options);
            await _database.StringSetAsync(Cashkey, SerializerResponse , ExpireTime);
        }

        public async Task<string> GetCacheResponse(string Cashkey)
        {
           var CashCacheResponse = await _database.StringGetAsync(Cashkey);
            if (CashCacheResponse.IsNullOrEmpty) return null;
            return CashCacheResponse;
        }
    }
}
