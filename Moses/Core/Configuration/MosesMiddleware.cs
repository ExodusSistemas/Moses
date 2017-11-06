using Microsoft.AspNetCore.Http;
using Moses.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiddlewareFunc = System.Func<Microsoft.AspNetCore.Http.RequestDelegate, Microsoft.AspNetCore.Http.RequestDelegate>;

namespace Moses
{

    /// <summary>
    /// Moses middleware for OWIN.
    /// </summary>
    public class MosesMiddleware
    {
        RequestDelegate _next;

        public IDictionary<string, string> SetHeaders { get; } = new Dictionary<string, string>();

        public ISet<string> RemoveHeaders { get; } = new HashSet<string>();

        public MosesMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.Headers.Add("powered-by-Moses", "The one who guides Exodus");
            await _next(context);
        }

    }
}

