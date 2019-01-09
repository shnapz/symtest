namespace symtest.Common.Models
{
    using System.Collections.Generic;
    using System.Net.Http;

    public class HttpRequestTemplate
    {
        public string Url { get; set; }
        public HttpMethod Method { get; set; }
        public Dictionary<string, string> Headers { get; set; }
    }
}