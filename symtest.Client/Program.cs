namespace symtest.Client
{
    using System;
    using System.IO;
    using System.Text;
    using Logic;
    using Microsoft.Extensions.Configuration;
    using RabbitMQ.Client;

    class Program
    {
        static void Main(string[] args)
        {
            var configuration = GetConfiguration();

            var requestReader = new RequestReader(configuration["ConfigFile"]);
            var requestTemplates = requestReader.GetRequestTemplates();
            
            var factory = new ConnectionFactory { HostName = configuration["Host"] };
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: configuration["Queue"],
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                string message = "Hello World!";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                    routingKey: configuration["Queue"],
                    basicProperties: null,
                    body: body);
                
                Console.WriteLine(" [x] Sent {0}", message);
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        static IConfiguration GetConfiguration()
            => new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
    }
}