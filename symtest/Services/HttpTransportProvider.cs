namespace symtest.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Common.Models;
    using Providers;

    public class HttpTransportProvider : IHttpTransportProvider
    {
        private readonly HttpClient _client;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IList<HttpRequestTemplate> _defaultTemplates;
        
        public HttpTransportProvider(IHttpClientFactory clientFactory,
                                     List<HttpRequestTemplate> defaultTemplates)
        {
            _clientFactory = clientFactory;
            _client = _clientFactory.CreateClient();
            
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
            var request = new HttpRequestMessage(requestTemplate.Method, 
                requestTemplate.Url);

            foreach (var header in requestTemplate.Headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }

            var response = await _client.SendAsync(request);
            
            return HttpStatusCode.Accepted;
        }
    }
}