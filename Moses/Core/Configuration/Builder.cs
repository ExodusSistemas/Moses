using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Moses.Configuration;
using System.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Moses
{
    interface IMosesBuilder
    {

    }

    public static class MosesBuilder
    {
        public static IApplicationBuilder UseMoses(this IApplicationBuilder app, ILoggerFactory factory)
        {
            Moses.Services.LogService.LoggerFactory = factory;
            Moses.Services.LogService.Log = factory.CreateLogger("Moses");
            return app.UseMiddleware<MosesMiddleware>();
        }

        public static void AddMoses(this IServiceCollection services, IConfiguration config,  MosesServiceOptions options = null)
        {
            services.AddOptions();
            var mosesOptions = options ?? new MosesServiceOptions();
            
            if (config.GetSection("Moses").Exists())
            {
                config.GetSection("Moses").Bind(mosesOptions.AppConfiguration);
            }

            services.AddSingleton<MosesServiceOptions>(mosesOptions);
            MosesServiceCollection.ConfigureServices(services);
        }
       
    }

}
