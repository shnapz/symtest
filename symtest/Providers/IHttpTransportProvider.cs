namespace symtest.Providers
{
    using System.Net;
    using System.Threading.Tasks;
    using Common.Models;

    public interface IHttpTransportProvider
    {
        Task<HttpStatusCode> ExecuteTest(HttpRequestTemplate requestTemplate);
    }
}