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

    public class HttpTransportProvider : IHttpTransportProvider
    {
        private readonly HttpClient _client;
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpRequestTemplate[] _defaultTemplates;
        private readonly Random _random;
        
        public HttpTransportProvider(IHttpClientFactory clientFactory,
                                     HttpRequestTemplate[] defaultTemplates)
        {
            _clientFactory = clientFactory;
            _client = _clientFactory.CreateClient();
            _random = new Random();
            
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
            
            var result = await RepeatActionEvery(ExecuteRequest, 
                                                 TimeSpan.FromSeconds(1),
                                                 requestTemplate,
                                                 Math.Pow(requestTemplate.Distribution, 1 / requestTemplate.Density),
                                                 cancellation.Token);
            
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

                await task;

                return result;
            }
        }

        private async Task<HttpStatusCode?> ExecuteRequest(HttpRequestTemplate requestTemplate,
                                                           double requestInIntervalProbability)
        {
            double diceRoll = _random.NextDouble();

            if (diceRoll < requestInIntervalProbability)
            {
                var request = new HttpRequestMessage((HttpMethod)Enum.Parse(typeof(HttpMethod), requestTemplate.Method),
                    requestTemplate.Url);

                foreach (var header in requestTemplate.Headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }

                var response = await _client.SendAsync(request);

                return response.StatusCode;
            }

            return null;
        }
    }
}