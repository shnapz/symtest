namespace symtest.Tests.symtest.Providers
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using global::symtest.Providers;
    using Microsoft.Extensions.Logging;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class HttpTransportProviderTests
    {
        private IHttpClientFactory _httpClientFactoryMock;
        private ILogger<HttpTransportProvider> _httpTransportProviderLoggerMock;

        [SetUp]
        public void SetUp()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>().Object;
            _httpTransportProviderLoggerMock = new Mock<ILogger<HttpTransportProvider>>().Object;
        }

        [TearDown]
        public void TearDown()
        {
            _httpClientFactoryMock = null;
            _httpTransportProviderLoggerMock = null;
        }

        [Test]
        public void Stub()
        {
            Assert.IsTrue(true);
        }
        
        [TestCaseSource(nameof(GetDataForSuccessfulTests))]
        public Task TestRequests()
        {
            
            
            
            return null;
        }

        public static IEnumerable<TestCaseData> GetDataForSuccessfulTests
        {
            get
            {
                yield return new TestCaseData();
            }
        }
    }
}