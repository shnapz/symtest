namespace symtest.Client.Logic
{
    using System.Collections.Generic;
    using System.IO;
    using Common.Models;
    using Newtonsoft.Json;

    public class RequestReader
    {
        private readonly string _fileName;
        
        public RequestReader(string fileName)
        {
            _fileName = fileName;
        }

        public List<HttpRequestTemplate> GetRequestTemplates()
        {
            using (StreamReader file = File.OpenText(_fileName))
            {
                JsonSerializer serializer = new JsonSerializer();
                List<HttpRequestTemplate> templates = (List<HttpRequestTemplate>)
                    serializer.Deserialize(file, typeof(List<HttpRequestTemplate>));

                return templates;
            }
        }
    }
}