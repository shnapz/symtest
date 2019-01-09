namespace symtest.Providers
{
    using Common.Models;

    public interface IHttpTransportProvider
    {
        string ExecuteTest(HttpRequestTemplate requestTemplate);
    }
}