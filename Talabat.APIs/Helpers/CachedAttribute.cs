using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Talabat.Core.Services;

namespace Talabat.APIs.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _expireTimeInSecond;
        public CachedAttribute(int ExpireTimeInSecond)
        {
            _expireTimeInSecond  = ExpireTimeInSecond;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
           var CachService =  context.HttpContext.RequestServices.GetRequiredService<IResponseCashService>();
            var Cachekey = GenerateCachekeyFromRequest(context.HttpContext.Request);
          var CachedResponse = await   CachService.GetCacheResponse(Cachekey);
            if (!string.IsNullOrEmpty(CachedResponse))
            {
                var contentResult = new ContentResult()
                {
                    Content = CachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200,
                    



                };
                context.Result = contentResult;
                return; 
            }

          var ExecutedEndpointContext =   await next.Invoke();
            if (ExecutedEndpointContext.Result is OkObjectResult result)
            {
                await CachService.CacheResponseAsync(Cachekey, result.Value, TimeSpan.FromSeconds(_expireTimeInSecond));
            }
            
        }

        private string  GenerateCachekeyFromRequest(HttpRequest request)
        {

            var KeyBuilder = new StringBuilder();
            KeyBuilder.Append(request.Path);
            foreach (var (key, value) in request.Query.OrderBy(x=>x.Key))
            {
                KeyBuilder.Append($"{key}-{value} ");
                
            }
            return KeyBuilder.ToString();
        }
    }
}
