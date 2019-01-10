namespace symtest.Listeners
{
    using System.Text;
    using RabbitMQ.Client.Events;

    public class RabbitListener : BaseRabbitListener
    {
        public RabbitListener(string hostName, 
                              string queueName) : base(hostName, queueName) {}
        
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