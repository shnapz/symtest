namespace symtest.Listeners
{
    using System;
    using System.Text;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    public class RabbitListener
    {
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        private readonly string _queueName;
        
        public RabbitListener(string hostName, 
                              string queueName)
        {
            _queueName = queueName ?? throw new ArgumentNullException(nameof(queueName));
            
            _factory = new ConnectionFactory { HostName = hostName };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
        }
        
        public void Register()
        {
            _channel.QueueDeclare(queue: _queueName, 
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
            };
            
            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
        }

        public void DeRegister()
            => _connection.Close();
    }
}