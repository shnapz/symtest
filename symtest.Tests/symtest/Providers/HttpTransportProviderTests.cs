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
        
        [TestCaseSource(nameof(GetDataForSuccessfulTests))]
        public async Task SuccessRequestsTests(string param)
        {
            
            
            
            Assert.IsTrue(true);
        }

        public static IEnumerable<TestCaseData> GetDataForSuccessfulTests
        {
            get
            {
                yield return new TestCaseData("dummy_string");
            }
        }
        
        [TestCaseSource(nameof(GetDataForFailedTests))]
        public async Task FailedRequestsTests(string param)
        {
            
            
            
            Assert.IsTrue(true);
        }

        public static IEnumerable<TestCaseData> GetDataForFailedTests
        {
            get
            {
                yield return new TestCaseData("dummy_string");
            }
        }
    }
}