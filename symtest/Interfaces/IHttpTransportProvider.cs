namespace symtest.Interfaces
{
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using Common.Models;

    public interface IHttpTransportProvider
    {
        Task<List<HttpStatusCode?>> ExecuteAllTests();
        Task<List<HttpStatusCode?>> ExecuteTest(HttpRequestTemplate requestTemplate);
    }
}