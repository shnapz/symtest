using System.Collections.Generic;

namespace Client.Infrastructure
{
    public sealed class AppSettings
    {
        public ServiceBusConnection ServiceBusConnection { get; set; }
        public TemplatesMessages TemplatesMessages { get; set; }
    }

    public sealed class TemplatesMessages
    {
        public IList<EndPoints> EndPointsList { get; set; }
    }

    public sealed class EndPoints
    {
        public string EndpointUrl { get; set; }
        public double Probability { get; set; }
        public string Message { get; set; }
    }

    public sealed class ServiceBusConnection
    {
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}