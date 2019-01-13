namespace symtest
{
    using System.Collections.Generic;
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
                
                return new HttpTransportProvider(clientFactory, GetDefaultTemplates(Configuration));
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

            app.UseRabbitListener();
        }

        public void InitModules()
        {
            
        }

        public List<HttpRequestTemplate> GetDefaultTemplates(IConfiguration configuration)
        {

            return null;
        }
    }
}