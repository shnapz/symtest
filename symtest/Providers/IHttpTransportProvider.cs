namespace symtest.Providers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using Common.Models;

    public interface IHttpTransportProvider
    {
        Task<List<HttpStatusCode?>> ExecuteAllTests();
        Task<HttpStatusCode?> ExecuteTest(HttpRequestTemplate requestTemplate);
    }
}