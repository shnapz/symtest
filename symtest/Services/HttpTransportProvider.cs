namespace symtest.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Timers;
    using Common.Models;
    using Providers;

    public class HttpTransportProvider : IHttpTransportProvider
    {
        private readonly HttpClient _client;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IList<HttpRequestTemplate> _defaultTemplates;
        private readonly Random _random;
        
        public HttpTransportProvider(IHttpClientFactory clientFactory,
                                     List<HttpRequestTemplate> defaultTemplates)
        {
            _clientFactory = clientFactory;
            _client = _clientFactory.CreateClient();
            _random = new Random();
            
            _defaultTemplates = defaultTemplates != null && defaultTemplates.Count > 0
                ? defaultTemplates
                : throw new ArgumentException(nameof(defaultTemplates));
        }

        public async Task<HttpStatusCode> ExecuteAllTests()
        {
            foreach (var requestTemplate in _defaultTemplates)
            {
                await ExecuteTest(requestTemplate);
            }
            
            return HttpStatusCode.Accepted;
        }
        
        public async Task<HttpStatusCode> ExecuteTest(HttpRequestTemplate requestTemplate)
        {
            Timer timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(ExecuteRequest);
            timer.Interval = 5000;
            timer.Enabled = true;
            
            double diceRoll = _random.NextDouble();

            if (diceRoll < requestTemplate.Distribution)
            {
                var request = new HttpRequestMessage(requestTemplate.Method,
                    requestTemplate.Url);

                foreach (var header in requestTemplate.Headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }

                var response = await _client.SendAsync(request);

            }

            return HttpStatusCode.Accepted;
        }

        private void ExecuteRequest(object source, ElapsedEventArgs e)
        {
            
        }
    }
}