namespace symtest.Common.Models
{
    using System.Collections.Generic;

    public class TransportConfiguration
    {
        public string Type { get; set; }
        public HttpRequestTemplate[] Templates { get; set; }
    }
}