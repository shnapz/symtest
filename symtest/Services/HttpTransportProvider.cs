namespace symtest.Services
{
    using System;
    using System.Collections.Generic;
    using Common.Models;
    using Providers;

    public class HttpTransportProvider : IHttpTransportProvider
    {
        private readonly IList<HttpRequestTemplate> _defaultTemplates;
        
        public HttpTransportProvider(List<HttpRequestTemplate> defaultTemplates)
        {
            _defaultTemplates = defaultTemplates != null && defaultTemplates.Count > 0
                ? defaultTemplates
                : throw new ArgumentException(nameof(defaultTemplates));
        }

        public void ExecuteAllTests()
        {
            
        }
        
        public string ExecuteTest(HttpRequestTemplate requestTemplate)
        {
            throw new System.NotImplementedException();
        }
    }
}