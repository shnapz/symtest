namespace symtest.Providers
{
    using System;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    public abstract class BaseRabbitListener
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        private readonly string _queueName;
        
        protected BaseRabbitListener(string hostName, 
            string queueName)
        {
            _queueName = queueName != null && !string.IsNullOrEmpty(queueName) ? queueName : throw new ArgumentException(nameof(queueName));
            
            var factory = new ConnectionFactory
            {
                HostName = hostName != null && !string.IsNullOrEmpty(hostName) ? hostName : throw new ArgumentException(nameof(hostName))
            };
            
            _connection = factory.CreateConnection();
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
                        
            HandleMessage(consumer);
            
            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
        }

        public void DeRegister()
            => _connection.Close();

        public abstract void HandleMessage(EventingBasicConsumer consumer);
    }
}