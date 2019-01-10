namespace symtest.Common.Models
{
    using System.Collections.Generic;

    public class TransportConfiguration
    {
        public string Type { get; set; }
        public List<HttpRequestTemplate> Endpoints { get; set; }
    }
}