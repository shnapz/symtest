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

        public TransportConfiguration[]  GetRequestTemplates()
        {
            using (StreamReader file = File.OpenText(_fileName))
            {
                JsonSerializer serializer = new JsonSerializer();
                TransportConfiguration[] templates = (TransportConfiguration[])
                    serializer.Deserialize(file, typeof(TransportConfiguration[]));

                return templates;
            }
        }
    }
}