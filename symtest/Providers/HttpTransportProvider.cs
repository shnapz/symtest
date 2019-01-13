namespace symtest.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
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
                results.Add(requestResult);
            }
            
            return results;
        }
        
        public async Task<HttpStatusCode?> ExecuteTest(HttpRequestTemplate requestTemplate)
        {
            CancellationTokenSource cancellation = new CancellationTokenSource(
                TimeSpan.FromMilliseconds(requestTemplate.Duration));
            
            _logger.LogInformation($"Executing TEST with URL {requestTemplate.Url} " +
                                   $"and METHOD {requestTemplate.Method}.");
            
            var result = await RepeatActionEvery(ExecuteRequest, 
                                                 TimeSpan.FromMilliseconds(requestTemplate.Duration / requestTemplate.Density),
                                                 requestTemplate,
                                                 Math.Pow(requestTemplate.Distribution, 1 / requestTemplate.Density),
                                                 cancellation.Token);
            if(result != null)
            {
                _logger.LogInformation($"TEST with URL {requestTemplate.Url} and METHOD {requestTemplate.Method}" +
                                       $" has been executed with {result} result.");
            }
            else
            {
                _logger.LogInformation($"TEST with URL {requestTemplate.Url} and METHOD {requestTemplate.Method}" +
                                       " has been executed without result.");  
            }
            
            
            return result;
        }

        private async Task<HttpStatusCode?> RepeatActionEvery(Func<HttpRequestTemplate, double, Task<HttpStatusCode?>> action, 
                                                              TimeSpan interval,
                                                              HttpRequestTemplate requestTemplate,
                                                              double requestInIntervalProbability,
                                                              CancellationToken cancellationToken)
        {
            while (true)
            {                
                var result = await action(requestTemplate, requestInIntervalProbability);
                Task task = Task.Delay(interval, cancellationToken);

                try
                {
                    await task;
                }
                catch (TaskCanceledException)
                {
                    return null;
                }
                
                return result;
            }
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

            return null;
        }
    }
}