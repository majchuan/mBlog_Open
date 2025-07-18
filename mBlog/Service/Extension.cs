using System;
using Microsoft.AspNetCore.Builder;

namespace mBlog.Service
{
    public static class Extension
    {
        public static IApplicationBuilder UseForwardHttpToHttps(this IApplicationBuilder mblogBuilder, Action<HttpToHttpsOption> config)
        {
            var options = new HttpToHttpsOption();
            config(options);
            return mblogBuilder.UseMiddleware<ForwardHttpToHttpsMiddleware>(options);
        }
    }
}