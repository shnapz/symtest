namespace symtest.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Common.Models;
    using Interfaces;
    using Microsoft.Extensions.Logging;

    public class HttpTransportProvider : IHttpTransportProvider
    {
        private readonly HttpClient _client;
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpRequestTemplate[] _defaultTemplates;
        private readonly Random _random;
        private readonly ILogger _logger;
        
        public HttpTransportProvider(ILogger<HttpTransportProvider> logger,
                                     IHttpClientFactory clientFactory,
                                     HttpRequestTemplate[] defaultTemplates)
        {
            _clientFactory = clientFactory;
            _client = _clientFactory.CreateClient();
            _random = new Random();
            _logger = logger;
            
            _defaultTemplates = defaultTemplates != null && defaultTemplates.Length > 0
                ? defaultTemplates
                : throw new ArgumentException(nameof(defaultTemplates));
        }

        public async Task<List<HttpStatusCode?>> ExecuteAllTests()
        {
            var results = new List<HttpStatusCode?>();
            
            foreach (var requestTemplate in _defaultTemplates)
            {
                var requestResult = await ExecuteTest(requestTemplate);
                results.AddRange(requestResult);
            }
            
            return results;
        }
        
        public async Task<List<HttpStatusCode?>> ExecuteTest(HttpRequestTemplate requestTemplate)
        {
            CancellationTokenSource cancellation = new CancellationTokenSource(
                TimeSpan.FromMilliseconds(requestTemplate.Duration));
            
            _logger.LogInformation($"Executing TEST with URL {requestTemplate.Url} " +
                                   $"and METHOD {requestTemplate.Method}.");
            
            var result = await RepeatActionEvery(ExecuteRequest, 
                                                 TimeSpan.FromMilliseconds(requestTemplate.Duration / (double)requestTemplate.Density),
                                                 requestTemplate,
                                                 Math.Pow(requestTemplate.Distribution, (double)1 / (double)requestTemplate.Density),
                                                 cancellation.Token);
            if(result != null)
            {
                _logger.LogInformation($"TEST with URL {requestTemplate.Url} and METHOD {requestTemplate.Method}" +
                                       $" has been executed with {string.Join(", ", result.Select(x => x == null ? "REQUEST WAS NOT EXECUTED" : x.ToString()))} result.");
            }
            else
            {
                _logger.LogInformation($"TEST with URL {requestTemplate.Url} and METHOD {requestTemplate.Method}" +
                                       " has been executed without result.");  
            }
            
            
            return result;
        }

        private async Task<List<HttpStatusCode?>> RepeatActionEvery(Func<HttpRequestTemplate, double, Task<HttpStatusCode?>> action, 
                                                              TimeSpan interval,
                                                              HttpRequestTemplate requestTemplate,
                                                              double requestInIntervalProbability,
                                                              CancellationToken cancellationToken)
        {
            var results = new List<HttpStatusCode?>();
            
            while (true)
            {                
                var result = await action(requestTemplate, requestInIntervalProbability);

                if (result != null)
                {
                    Task task = Task.Delay(interval, cancellationToken);

                    try
                    {
                        await task;
                    }
                    catch (TaskCanceledException)
                    {
                        break;
                    }
                }

                results.Add(result);
            }

            return results;
        }

        private async Task<HttpStatusCode?> ExecuteRequest(HttpRequestTemplate requestTemplate,
                                                           double requestInIntervalProbability)
        {
            double diceRoll = _random.NextDouble();

            if (diceRoll < requestInIntervalProbability)
            {
                var request = new HttpRequestMessage(new HttpMethod(requestTemplate.Method),
                    requestTemplate.Url);

                foreach (var header in requestTemplate.Headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }

                _logger.LogInformation("Prepared request object. Sending request...");
                
                var response = await _client.SendAsync(request);

                _logger.LogInformation($"Received response with status code {response.StatusCode}.");
                

                return response.StatusCode;
            }
            
            _logger.LogInformation("REQUEST ARE NOT GOING TO BE EXECUTED!");  
            
            return null;
        }
    }
}