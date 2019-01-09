namespace symtest.Providers
{
    using Models;

    public interface IHttpTransportProvider
    {
        string ExecuteTest(HttpRequestTemplate requestTemplate);
    }
}