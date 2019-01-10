namespace symtest
{
    using Extensions;
    using Listeners;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Providers;
    using Services;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddTransient<IHttpTransportProvider>(s => new HttpTransportProvider(Configuration[]));
            
            services.AddSingleton<RabbitListener>(s => new RabbitListener(Configuration["Host"], Configuration["Queue"]));
            services.AddHttpClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRabbitListener();
            
            //app.Run(async (context) => { await context.Response.WriteAsync("Hello World!"); });
        }

        public void InitModules()
        {
            
        }

        public void GetDefaultTemplates()
        {
            
        }
    }
}