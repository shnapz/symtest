namespace symtest
{
    using System.Net.Http;
    using Common.Models;
    using Extensions;
    using Interfaces;
    using Listeners;
    using Listeners.Base;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Middleware;
    using Providers;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            
            services.AddSingleton<IHttpTransportProvider>(context =>
            {
                IHttpClientFactory clientFactory = context.GetService<IHttpClientFactory>();
                ILogger<HttpTransportProvider> logger = context.GetService<ILogger<HttpTransportProvider>>();
                
                return new HttpTransportProvider(logger, clientFactory, GetDefaultTemplates(Configuration));
            });

            services.AddSingleton<BaseRabbitListener>(
                context =>
                {
                    IHttpTransportProvider transportProvider = context.GetService<IHttpTransportProvider>();
                    return new RabbitListener(transportProvider, Configuration["Host"], Configuration["Queue"]);
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware(typeof(ExceptionHandlingMiddleware));
            app.UseRabbitListener();
        }

        public HttpRequestTemplate[] GetDefaultTemplates(IConfiguration configuration)
        {
            var transportSection = configuration.GetSection("Transport");
            var transportConfigurationData = transportSection.Get<TransportConfiguration[]>();

            return transportConfigurationData[0].Templates;
        }
    }
}