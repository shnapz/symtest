namespace symtest.Client
{
    using System;
    using System.IO;
    using System.Text;
    using Common;
    using Common.Models;
    using Logic;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using RabbitMQ.Client;

    class Program
    {
        static void Main(string[] args)
        {
            var configuration = GetConfiguration();
            
            HttpRequestTemplate[] templates = null;

            if (bool.Parse(configuration["UseConfig"]))
            {
                var requestReader = new RequestReader(configuration["ConfigFile"]);
                templates = requestReader.GetRequestTemplates()[0].Templates;
            }
            
            var factory = new ConnectionFactory { HostName = configuration["Host"] };
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: configuration["Queue"],
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                if (templates != null)
                {
                    foreach (var template in templates)
                    {
                        string message  = JsonConvert.SerializeObject(template);
                        var body = Encoding.UTF8.GetBytes(message);
                        
                        channel.BasicPublish(exchange: "",
                            routingKey: configuration["Queue"],
                            basicProperties: null,
                            body: body);
                        
                        Console.WriteLine($"Sent template with URL: {template.Url} ; Method: {template.Method}");
                    }
                }
                else
                {
                    string message = Constants.UseProvidedTemplates;
                    var body = Encoding.UTF8.GetBytes(message);
                    
                    channel.BasicPublish(exchange: "",
                        routingKey: configuration["Queue"],
                        basicProperties: null,
                        body: body);
                }
            }

            Console.WriteLine("Sent all data to broker.");
            Console.ReadLine();
        }

        static IConfiguration GetConfiguration()
            => new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
    }
}