namespace symtest.Tests.symtest.Providers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    public class HttpTransportProviderTests
    {
        [Test]
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