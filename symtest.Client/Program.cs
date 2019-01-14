namespace symtest.Client
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using Common;
    using Common.Models;
    using Logic;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;

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
            
            var rpcClient = new RpcClient(configuration["Host"], configuration["Queue"]);
            
            if (templates != null)
            {
                foreach (var template in templates)
                {
                    string message  = JsonConvert.SerializeObject(template);
                    var body = Encoding.UTF8.GetBytes(message);

                    Console.WriteLine($"Sent template with URL: {template.Url} ; Method: {template.Method}");
                    
                    var response = rpcClient.Call(body);
                    
                    var statusCodes = JsonConvert.DeserializeObject<HttpStatusCode?[]>(response);
                    
                    Console.WriteLine($"Response is {string.Join(", ", statusCodes.Select(x => x == null ? "REQUEST WERE NOT EXECUTED" : x.ToString()))}");
                }
            }
            else
            {
                Console.WriteLine($"Using templates provided on server...");
                
                string message = Constants.UseProvidedTemplates;
                var body = Encoding.UTF8.GetBytes(message);
                
                var response = rpcClient.Call(body);
                
                var statusCodes = JsonConvert.DeserializeObject<List<HttpStatusCode?>>(response);

                Console.WriteLine($"Response is {string.Join(", ", statusCodes.Select(x => x == null ? "REQUEST WERE NOT EXECUTED" : x.ToString()))}");
            }
            
            Console.WriteLine("Sent all data to services...");
            Console.ReadLine();
            rpcClient.Close();
        }

        static IConfiguration GetConfiguration()
            => new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
    }
}