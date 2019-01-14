namespace symtest.Listeners.Base
{
    using System;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    public abstract class BaseRabbitListener
    {
        protected readonly string QueueName;
        protected readonly string HostName;
        protected readonly IConnection Connection;
        protected readonly IModel Channel;
        
        protected BaseRabbitListener(string hostName, 
            string queueName)
        {
            QueueName = queueName != null && !string.IsNullOrEmpty(queueName) ? queueName : throw new ArgumentException(nameof(queueName));
            HostName = hostName != null && !string.IsNullOrEmpty(hostName) ? hostName : throw new ArgumentException(nameof(hostName));
            
            var factory = new ConnectionFactory { HostName = HostName };
            Connection = factory.CreateConnection();
            Channel = Connection.CreateModel();
        }
        
        public void Register()
        {
            HandleMessage();
        }

        public void DeRegister()
        {
            Channel.Close();
            Connection.Close();
        }

        protected abstract void HandleMessage();
    }
}