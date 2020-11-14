using System;
using System.Text.RegularExpressions;
using NUnit.Framework;
using SWE1_MTCG.Api;
using SWE1_MTCG.Server;

namespace SWE1_MTCG.Test
{
    [TestFixture]
    public class RequestContextTests
    {
        [Test]
        public void TestRequestContextRightMethod()
        {
            string requestString = "GET /messages HTTP/1.1\r\nUser-Agent: curl/7.55.1\r\nAccept: */*";
           
            RequestContext request = new RequestContext(requestString);

            Assert.AreEqual("GET", request.HttpMethod);
        }

        [Test]
        public void TestRequestContextRightResource()
        {
            string requestString = "GET /messages HTTP/1.1\r\nUser-Agent: curl/7.55.1\r\nAccept: */*";

            RequestContext request = new RequestContext(requestString);

            Assert.AreEqual("/messages", request.RequestedResource);
        }

        [Test]
        public void TestRequestContextRightVersion()
        {
            string requestString = "GET /messages HTTP/1.1\r\nUser-Agent: curl/7.55.1\r\nAccept: */*";

            RequestContext request = new RequestContext(requestString);

            Assert.AreEqual("HTTP/1.1", request.HttpVersion);
        }
    }
}