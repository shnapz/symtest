namespace symtest.Common.Models
{
    using System.Collections.Generic;
    using System.Net.Http;

    public class HttpRequestTemplate
    {
        public string Url { get; set; }
        public string Method { get; set; }
        
        public int Density { get; set; }
        public double Duration { get; set; }
        public double Distribution { get; set; }
        
        public Dictionary<string, string> Headers { get; set; }
    }
}