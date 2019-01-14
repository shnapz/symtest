namespace symtest.Client.Logic
{
    using System;
    using System.Collections.Concurrent;
    using System.Text;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    public class RpcClient
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _replyQueueName;
        private readonly EventingBasicConsumer _consumer;
        private readonly string _queueName;
        private readonly BlockingCollection<string> _respQueue = new BlockingCollection<string>();
        private readonly IBasicProperties _props;

        public RpcClient(string hostName, string queueName)
        {
            _queueName = queueName;
            
            var factory = new ConnectionFactory() {HostName = hostName};

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _replyQueueName = _channel.QueueDeclare().QueueName;
            _consumer = new EventingBasicConsumer(_channel);

            _props = _channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            _props.CorrelationId = correlationId;
            _props.ReplyTo = _replyQueueName;

            _consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var response = Encoding.UTF8.GetString(body);
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    _respQueue.Add(response);
                }
            };
        }

        public string Call(byte[] message)
        {
            _channel.BasicPublish(
                exchange: "",
                routingKey:_queueName,
                basicProperties: _props,
                body: message);

            _channel.BasicConsume(
                consumer: _consumer,
                queue: _replyQueueName,
                autoAck: true);

            return _respQueue.Take();
        }

        public void Close()
            => _connection.Close();
    }
}