using System;
using System.Text.RegularExpressions;
using NUnit.Framework;
using SWE1_MTCG.Api;
using SWE1_MTCG.Server;

namespace SWE1_MTCG.Test
{
    [TestFixture]
    public class MessageApiTests
    {
        /*[SetUp]
        public void Setup()
        {

        }*/

        [Test]
        public void TestMessageApiReturnsNoError()
        {
            string requestString = "GET /messages/ HTTP/1.1\r\nUser-Agent: curl/7.55.1\r\nAccept: */*";
            RequestContext request = new RequestContext(requestString);
            Regex messageRegex = new Regex(@"^/messages/?\d*$");

            MessageApi api = new MessageApi(request, messageRegex);
            string response = api.Interaction();

            Assert.AreNotEqual("GET ERR", response);
        }

        [Test]
        public void TestMessageApiReturnsError()
        {
            string requestString = "GET /messages/100 HTTP/1.1\r\nUser-Agent: curl/7.55.1\r\nAccept: */*";
            RequestContext request = new RequestContext(requestString);
            Regex messageRegex = new Regex(@"^/messages/?\d*$");

            MessageApi api = new MessageApi(request, messageRegex);
            string response = api.Interaction();

            Assert.AreEqual("GET ERR", response);
        }
    }
}