using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moses.Configuration;
using Moses.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Moses
{
    /// <summary>
    /// Apply Moses Default services to the service collection
    /// </summary>
    public static class MosesServiceCollection
    {
        public static void ConfigureServices(IServiceCollection services, IServiceOptions options = null, IEmailService emailService = null )
        {
            options = options ?? new MosesServiceOptions();

            // Register email service 
            services.AddSingleton(options);
            services.AddSingleton<IEmailService>(emailService ?? new EmailService());

            
        }

    }
}
