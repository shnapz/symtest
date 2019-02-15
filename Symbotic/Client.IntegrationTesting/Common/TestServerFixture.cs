using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;

namespace Client.IntegrationTesting.Common
{
    public class TestServerFixture : IDisposable
    {
        private readonly TestServer _testServer;
        private bool disposed = false;

        public HttpClient Client { get; }

        public TestServerFixture()
        {
            var projectDir = System.IO.Directory.GetCurrentDirectory();

            var builder = new WebHostBuilder()
                .UseEnvironment("Development")
                //.UseContentRoot(projectDir)
                .UseConfiguration(new ConfigurationBuilder()
                    //.SetBasePath(projectDir)
                    .AddJsonFile("appsettings.Development.json")
                    .Build())
                .UseStartup<Startup>();

            _testServer = new TestServer(builder);
            Client = _testServer.CreateClient();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                Client.Dispose();
                _testServer.Dispose();
            }

            disposed = true;
        }
    }
}