using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;


namespace mBlog.Service
{
    public class ForwardHttpToHttpsMiddleware
    {
        public readonly RequestDelegate _next ;
        public readonly HttpToHttpsOption _option;
        public ForwardHttpToHttpsMiddleware(RequestDelegate next, HttpToHttpsOption option )
        {
            this._next = next;
            this._option = option ;
        }

        public async Task Invoke(HttpContext context)
        {
            if(this._option.SwitchForwardToHtttps && (context.Request.IsHttps == false || context.Request.Headers["X-Forwarded-Proto"] == Uri.UriSchemeHttp ))
            {
                string queryString = context.Request.QueryString.HasValue ? context.Request.QueryString.Value : string.Empty;
                var https = "https://" + context.Request.Host + context.Request.Path + queryString;
                context.Response.Redirect(https);
            }else{
                await _next(context);
            }
        }

        
    }
}