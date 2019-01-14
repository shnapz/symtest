namespace symtest.Listeners
{
    using System;
    using System.Text;
    using Base;
    using Common;
    using Common.Models;
    using Interfaces;
    using Newtonsoft.Json;
    using RabbitMQ.Client;
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
        
        protected override void HandleMessage()
        {
            Channel.QueueDeclare(queue: QueueName, durable: false,
                    exclusive: false, autoDelete: false, arguments: null);
            Channel.BasicQos(0, 1, false);
                var consumer = new EventingBasicConsumer(Channel);
            Channel.BasicConsume(queue: QueueName,
                    autoAck: false, consumer: consumer);

            consumer.Received += (model, ea) =>
            {
                string response = null;

                var body = ea.Body;
                var props = ea.BasicProperties;
                var replyProps = Channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;

                try
                {
                    var message = Encoding.UTF8.GetString(body);

                    if (message == Constants.UseProvidedTemplates)
                    {
                        _httpTransportProvider.ExecuteAllTests();
                    }
                    else
                    {
                        HttpRequestTemplate templateToTest = JsonConvert.DeserializeObject<HttpRequestTemplate>(message);

                        response = _httpTransportProvider.ExecuteTest(templateToTest).Result.ToString();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(" [.] " + e.Message);
                    response = "";
                }
                finally
                {
                    var responseBytes = Encoding.UTF8.GetBytes(response);
                    Channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                        basicProperties: replyProps, body: responseBytes);
                    Channel.BasicAck(deliveryTag: ea.DeliveryTag,
                        multiple: false);
                }
            };
        }
    }
}