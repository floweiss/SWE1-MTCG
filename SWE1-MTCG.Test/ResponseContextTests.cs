using System;
using System.Text.RegularExpressions;
using NUnit.Framework;
using SWE1_MTCG.Api;
using SWE1_MTCG.Server;

namespace SWE1_MTCG.Test
{
    [TestFixture]
    public class ResponseContextTests
    {
        [Test]
        public void TestResponseContextRightStatusCode()
        {
            string requestString = "GET /messagesnew HTTP/1.1\r\nUser-Agent: curl/7.55.1\r\nAccept: */*";
            RequestContext request = new RequestContext(requestString);

            ResponseContext response = new ResponseContext(request, "Resource ERR");

            Assert.AreEqual(501, response.StatusCode);
        }

        [Test]
        public void TestResponseContextRightStatusString()
        {
            string requestString = "POST /messages HTTP/1.1\r\nUser-Agent: curl/7.55.1\r\nAccept: */*\r\nContent-Length: 7\r\n\r\nMessage";
            RequestContext request = new RequestContext(requestString);

            ResponseContext response = new ResponseContext(request, "POST OK - ID: 1");

            Assert.AreEqual("Created", response.StatusString);
        }

        [Test]
        public void TestResponseContextRightContentLength()
        {
            string requestString = "GET /messages/1 HTTP/1.1\r\nUser-Agent: curl/7.55.1\r\nAccept: */*";
            RequestContext request = new RequestContext(requestString);

            string content = "Message One";
            ResponseContext response = new ResponseContext(request, content);

            Assert.AreEqual(content.Length, response.ContentLength);
        }
    }
}