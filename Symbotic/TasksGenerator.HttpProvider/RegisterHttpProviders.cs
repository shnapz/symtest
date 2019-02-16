using Microsoft.Extensions.DependencyInjection;
using System.Net;
using TasksGenerator.HttpProvider.Providers;
using TasksGenerator.Infrastructure.Providers;

namespace TasksGenerator.HttpProvider
{
    public static class RegisterHttpProviders
    {
        public static IServiceCollection RegisterHttpProvider(this IServiceCollection services)
        {
            services.AddTransient<ITransportProvider<HttpStatusCode>, HttpTransportProvider>();

            return services;
        }
    }
}