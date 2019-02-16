using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Moq;
using TasksGenerator.HttpProvider.Providers;
using TasksGenerator.Infrastructure;
using Xunit;

namespace TasksGenerator.HttpProvider.Test
{
    public class HttpTransportProviderTest
    {
        public HttpTransportProvider _httpTransportProvider;
        Mock<IHttpClientFactory> _mockHttpClientFactory;
        Mock<IOptions<AppSettings>> _mockAppSettings;

        void Initialization()
        {

            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockAppSettings = new Mock<IOptions<AppSettings>>();

         _httpTransportProvider = new HttpTransportProvider(_mockHttpClientFactory.Object, _mockAppSettings.Object);
       }


        [Fact]
        public async Task  SendRequestExternalApiAsyncTest()
        {
            this.Initialization();


            _mockHttpClientFactory.Setup(x => x.CreateClient());

            //arrange
            //await _httpTransportProvider.SendRequestExternalApiAsync(It.IsAny<string>(), It.IsAny<string>());

            _mockHttpClientFactory.Verify(x=>x.CreateClient(), Times.AtLeastOnce);

        }

        



    }
}
