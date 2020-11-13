using NUnit.Framework;
using Moq;
using System;
using System.Text.RegularExpressions;
using SWE1_MTCG.Api;
using SWE1_MTCG.Server;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Test
{
    [TestFixture]
    public class MessageApiTests
    {
        private Mock<IFileService> _fileService;

        [SetUp]
        public void Setup()
        {
            _fileService = new Mock<IFileService>();
        }

        [Test]
        public void TestMessageApiReturnsNoError()
        {
            string requestString = "GET /messages/ HTTP/1.1\r\nUser-Agent: curl/7.55.1\r\nAccept: */*";
            RequestContext request = new RequestContext(requestString);
            Regex messageRegex = new Regex(@"^/messages/?\d*$");
            MessageApi api = new MessageApi(request, messageRegex, _fileService.Object);
            _fileService.Setup(s => s.ReadAllFilesInDir("sample")).Returns("0.txt: test\n1.txt: test2");

            string response = api.Interaction();

            Assert.AreNotEqual("GET ERR", response);
        }

        [Test]
        public void TestMessageApiReturnsError()
        {
            string requestString = "GET /messages/100 HTTP/1.1\r\nUser-Agent: curl/7.55.1\r\nAccept: */*";
            RequestContext request = new RequestContext(requestString);
            Regex messageRegex = new Regex(@"^/messages/?\d*$");
            MessageApi api = new MessageApi(request, messageRegex, _fileService.Object);
            _fileService.Setup(s => s.FileExists("sample")).Returns(false);

            string response = api.Interaction();

            Assert.AreEqual("GET ERR", response);
        }
    }
}