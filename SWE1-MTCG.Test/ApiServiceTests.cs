using System;
using System.Text.RegularExpressions;
using NUnit.Framework;
using SWE1_MTCG.Api;
using SWE1_MTCG.Server;

namespace SWE1_MTCG.Test
{
    [TestFixture]
    public class ApiServiceTests
    {
        private IApiService _apiService;

        [SetUp]
        public void Setup()
        {
            _apiService = new ApiService();
        }

        [Test]
        public void TestApiServiceReturnsNotNull()
        {
            string requestString = "GET /messages/ HTTP/1.1\r\nUser-Agent: curl/7.55.1\r\nAccept: */*";
            RequestContext request = new RequestContext(requestString);

            IApi api = _apiService.GetApi(request);

            Assert.IsNotNull(api);
        }

        [Test]
        public void TestApiServiceReturnsNullWhenWrongResource()
        {
            string requestString = "GET /messagesoje/ HTTP/1.1\r\nUser-Agent: curl/7.55.1\r\nAccept: */*";
            RequestContext request = new RequestContext(requestString);

            IApi api = _apiService.GetApi(request);

            Assert.IsNull(api);
        }
    }
}