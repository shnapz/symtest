namespace symtest.Tests.Integration.symtest.Providers
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net;
    using System.Text;
    using Client.Logic;
    using Common;
    using Common.Models;
    using Newtonsoft.Json;
    using NUnit.Framework;

    [TestFixture]
    public class HttpTransportProviderIntegrationTests
    {
        [Test]
        public void TestModuleIntegrationWithOneInstanceAndDataFromClient()
        {
            Process process = new Process
            {
                StartInfo =
                {
                    FileName = "launch_wrapper.sh",
                    Arguments = "1"
                }
            };
            
            process.Start();
            
            var rpcClient = new RpcClient("localhost", "rpc_queue");

            var template = new HttpRequestTemplate
            {
                Method = "GET",
                Url = "http://httpbin.org/ip",
                Density = 10,
                Duration = 1000,
                Distribution = 0.1,
                Headers = new Dictionary<string, string>()
            };
            
            string message  = JsonConvert.SerializeObject(template);
            var body = Encoding.UTF8.GetBytes(message);

            var result = rpcClient.Call(body);
            
            Process killProcess = new Process
            {
                StartInfo =
                {
                    FileName = "teardown_wrapper.sh",
                    Arguments = "1"
                }
            };
            
            killProcess.Start();
            
            Assert.IsTrue(result.Contains("200"));
        }
        
        [Test]
        public void TestModuleIntegrationWithTwoInstancesAndDataFromClient()
        {
            for (int i = 1; i < 3; i++)
            {
                Process process = new Process
                {
                    StartInfo =
                    {
                        FileName = "launch_wrapper.sh",
                        Arguments = i.ToString()
                    }
                };
            
                process.Start();
            }
            
            var rpcClient = new RpcClient("localhost", "rpc_queue");

            var templates = new List<HttpRequestTemplate>
            {
                new HttpRequestTemplate
                {
                    Method = "GET",
                    Url = "http://httpbin.org/ip",
                    Density = 10,
                    Duration = 1000,
                    Distribution = 0.1,
                    Headers = new Dictionary<string, string>()
                },
                new HttpRequestTemplate
                {
                    Method = "DELETE",
                    Url = "http://httpbin.org/ip",
                    Density = 10,
                    Duration = 1000,
                    Distribution = 0.1,
                    Headers = new Dictionary<string, string>()
                },
            };

            string results = string.Empty;
            
            foreach (var template in templates)
            {
                string message  = JsonConvert.SerializeObject(template);
                var body = Encoding.UTF8.GetBytes(message);

                var result = rpcClient.Call(body);
                results += result;
            }
            
            for (int i = 1; i < 3; i++)
            {
                Process process = new Process
                {
                    StartInfo =
                    {
                        FileName = "teardown_wrapper.sh",
                        Arguments = i.ToString()
                    }
                };
            
                process.Start();
            }
            
            Assert.IsTrue(results.Contains("200"));
            Assert.IsTrue(results.Contains("405"));
        }
        
        [Test]
        public void TestModuleIntegrationWithOneInstanceAndNoDataFromClient()
        {
            Process process = new Process
            {
                StartInfo =
                {
                    FileName = "launch_wrapper.sh",
                    Arguments = "1"
                }
            };
            
            process.Start();
            
            var rpcClient = new RpcClient("localhost", "rpc_queue");

            var body = Encoding.UTF8.GetBytes(Constants.UseProvidedTemplates);

            var result = rpcClient.Call(body);
            
            Process killProcess = new Process
            {
                StartInfo =
                {
                    FileName = "teardown_wrapper.sh",
                    Arguments = "1"
                }
            };
            
            killProcess.Start();
            
            Assert.IsTrue(result.Contains("404"));
        }
    }
}