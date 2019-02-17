using Client.Infrastructure.ServiceBus;
using Contracts;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Client.Infrastructure
{
    public static class RegisterServices
    {
        public static void RegisterMassTransit(this IServiceCollection services, AppSettings settings)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<StatisticHandler>();
                x.AddConsumer<StartExecutingTestHandler>();
            });

            services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(
                configurator =>
                {
                    var rabbitMqHost = configurator.Host(
                        new Uri(settings.ServiceBusConnection.Host),
                        hostConfigurator =>
                        {
                            hostConfigurator.Username(settings.ServiceBusConnection.UserName);
                            hostConfigurator.Password(settings.ServiceBusConnection.Password);
                        });
                    configurator.ReceiveEndpoint(rabbitMqHost, ServiceBusQueues.ClientTasks, endpointConfigurator =>
                    {
                        endpointConfigurator.LoadFrom(provider);
                    });
                }));

            services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());
        }

        public static void StartBusControl(this IServiceProvider serviceProvider)
        {
            var serviceBus = serviceProvider.GetRequiredService<IBusControl>();
            serviceBus.Start();
        }
    }
}