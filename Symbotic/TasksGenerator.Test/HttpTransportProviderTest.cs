//using Microsoft.Extensions.Options;
//using Moq;
//using System.Net.Http;
//using TasksGenerator.HttpProvider.Providers;
//using TasksGenerator.Infrastructure;

//namespace TasksGenerator.HttpProvider.Test
//{
//    public class HttpTransportProviderTest
//    {
//        public HttpTransportProvider _httpTransportProvider;
//        private Mock<IHttpClientFactory> _mockHttpClientFactory;
//        private Mock<IOptions<AppSettings>> _mockAppSettings;

//        private void Initialization()
//        {
//            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
//            _mockAppSettings = new Mock<IOptions<AppSettings>>();

//            _httpTransportProvider = new HttpTransportProvider(_mockHttpClientFactory.Object, _mockAppSettings.Object);
//        }

//        //[Fact]
//        //public async Task  SendRequestExternalApiAsyncTest()
//        //{
//        //    this.Initialization();

//        //    _mockHttpClientFactory.Setup(x => x.CreateClient());

//        //    //arrange
//        //    //await _httpTransportProvider.SendRequestExternalApiAsync(It.IsAny<string>(), It.IsAny<string>());

//        //    _mockHttpClientFactory.Verify(x=>x.CreateClient(), Times.AtLeastOnce);

//        //}

//        //[Fact]
//        //public async Task will_return_last_known_good()
//        //{
//        //    var firstCall = true;
//        //    var handler = new MockMessageHandler(req =>
//        //    {
//        //        if (firstCall)
//        //        {
//        //            firstCall = false;
//        //            var resp = new HttpResponseMessage(HttpStatusCode.OK);
//        //            return resp;
//        //        }
//        //        return new HttpResponseMessage(HttpStatusCode.InternalServerError);
//        //    });

//        //    var client = new HttpClient(handler);
//        //    client.BaseAddress = new Uri("http://test.local");

//        //    var service =   new ValuesService(client,  );

//        //    await service.GetValues();
//        //    var values = await service.GetValues();

//        //    Assert.False(firstCall);
//        //    Assert.Equal("testval", values.First());
//        //}

//        //public class MockMessageHandler : HttpMessageHandler
//        //{
//        //    private Func<HttpRequestMessage, HttpResponseMessage> _handler;

//        //    public MockMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> handler)
//        //    {
//        //        _handler = handler;
//        //    }

//        //    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
//        //    {
//        //        return Task.FromResult(_handler(request));
//        //    }
//        //}

//        //public async Task WhenACorrectUrlIsProvided_ServiceShouldReturnAlistOfUsers()
//        //{
//        //    // Arrange

//        //    var httpClientFactoryMock = new Mock<IHttpClientFactory>();
//        //    var url = "http://good.uri";

//        //    var fakeHttpMessageHandler = new FakeHttpMessageHandler(new HttpResponseMessage()
//        //    {
//        //        StatusCode = HttpStatusCode.OK,
//        //        //Content = new StringContent(JsonConvert.SerializeObject(users), Encoding.UTF8, "application/json")
//        //    });
//        //    var fakeHttpClient = new HttpClient(fakeHttpMessageHandler);

//        //    httpClientFactoryMock.Object.CreateClient();

//        //    // Act
//        //    var service = new UserService(httpClientFactoryMock);
//        //    var result = await service.GetUsers(url);

//        //    // Assert
//        //    result
//        //        .Should()
//        //        .And
//        //        .HaveCount(2);

//        //}
//    }
//}