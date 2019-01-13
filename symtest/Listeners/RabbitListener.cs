namespace symtest.Listeners
{
    using System;
    using System.Text;
    using Providers;
    using RabbitMQ.Client.Events;

    public class RabbitListener : BaseRabbitListener
    {
        private readonly IHttpTransportProvider _httpTransportProvider;
        
        public RabbitListener(IHttpTransportProvider httpTransportProvider,
            string hostName,
            string queueName) : base(hostName, queueName)
        {
            _httpTransportProvider =
                httpTransportProvider ?? throw new ArgumentNullException(nameof(httpTransportProvider));
        }
        
        public override void HandleMessage(EventingBasicConsumer consumer)
        {
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
            };
        }
    }
}