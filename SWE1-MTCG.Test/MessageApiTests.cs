using NUnit.Framework;
using Moq;
using System;
using System.IO;
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
        public void TestMessageApiGetAllMessages()
        {
            string requestString = "GET /messages/ HTTP/1.1\r\nUser-Agent: curl/7.55.1\r\nAccept: */*";
            RequestContext request = new RequestContext(requestString);
            Regex messageRegex = new Regex(@"^/messages/?\d*$");
            MessageApi api = new MessageApi(request, messageRegex, _fileService.Object, "messages");

            // FileService gets mocked
            _fileService.Setup(s => s.GetFilesInDir("messages"))
                .Returns(new string[] { "messages\\0.txt", "messages\\1.txt" });
            _fileService.Setup(s => s.ReadFromFile("messages\\0.txt")).Returns("Message0");
            _fileService.Setup(s => s.ReadFromFile("messages\\1.txt")).Returns("Message1");
            _fileService.Setup(s => s.ReadAllFilesInDir("messages")).Returns("0.txt: Message0\n1.txt: Message1");

            string response = api.Interaction();

            Assert.AreEqual("0.txt: Message0\n1.txt: Message1", response);
        }

        [Test]
        public void TestMessageApiGetOneMessage()
        {
            string requestString = "GET /messages/100 HTTP/1.1\r\nUser-Agent: curl/7.55.1\r\nAccept: */*";
            RequestContext request = new RequestContext(requestString);
            Regex messageRegex = new Regex(@"^/messages/?\d*$");
            MessageApi api = new MessageApi(request, messageRegex, _fileService.Object, "messages");

            _fileService.Setup(s => s.FileExists("messages\\100.txt")).Returns(true);
            _fileService.Setup(s => s.ReadFromFile("messages\\100.txt")).Returns("message");

            string response = api.Interaction();

            Assert.AreEqual("message", response);
        }

        [Test]
        public void TestMessageApiGetErrorWhenMessageNotExists()
        {
            string requestString = "GET /messages/100 HTTP/1.1\r\nUser-Agent: curl/7.55.1\r\nAccept: */*";
            RequestContext request = new RequestContext(requestString);
            Regex messageRegex = new Regex(@"^/messages/?\d*$");
            MessageApi api = new MessageApi(request, messageRegex, _fileService.Object, "messages");
            _fileService.Setup(s => s.FileExists("messages\\100.txt")).Returns(false);

            string response = api.Interaction();

            Assert.AreEqual("GET ERR", response);
        }

        [Test]
        public void TestMessageApiDeleteMessage()
        {
            string requestString = "DELETE /messages/1 HTTP/1.1\r\nUser-Agent: curl/7.55.1\r\nAccept: */*";
            RequestContext request = new RequestContext(requestString);
            Regex messageRegex = new Regex(@"^/messages/?\d*$");
            MessageApi api = new MessageApi(request, messageRegex, _fileService.Object, "messages");
            _fileService.Setup(s => s.FileExists("messages\\1.txt")).Returns(true);

            string response = api.Interaction();

            Assert.AreEqual("DELETE OK", response);
        }

        [Test]
        public void TestMessageApiDeleteErrorWhenMessageNotExists()
        {
            string requestString = "DELETE /messages/1 HTTP/1.1\r\nUser-Agent: curl/7.55.1\r\nAccept: */*";
            RequestContext request = new RequestContext(requestString);
            Regex messageRegex = new Regex(@"^/messages/?\d*$");
            MessageApi api = new MessageApi(request, messageRegex, _fileService.Object, "messages");
            _fileService.Setup(s => s.FileExists("messages\\1.txt")).Returns(false);

            string response = api.Interaction();

            Assert.AreEqual("DELETE ERR", response);
        }

        [Test]
        public void TestMessageApiPostCreatesMessage()
        {
            string requestString = "POST /messages HTTP/1.1\r\nUser-Agent: curl/7.55.1\r\nAccept: */*\r\nContent-Length: 7\r\n\r\nMessage";
            RequestContext request = new RequestContext(requestString);
            Regex messageRegex = new Regex(@"^/messages/?\d*$");
            MessageApi api = new MessageApi(request, messageRegex, _fileService.Object, "messages");
            _fileService.Setup(s => s.FileExists("messages\\1.txt")).Returns(false);

            string response = api.Interaction();

            Assert.AreEqual("POST OK - ID: 0", response);
        }

        [Test]
        public void TestMessageApiPutUpdatesMessage()
        {
            string requestString = "PUT /messages/1 HTTP/1.1\r\nUser-Agent: curl/7.55.1\r\nAccept: */*\r\nContent-Length: 19\r\n\r\nOverwritten Message";
            RequestContext request = new RequestContext(requestString);
            Regex messageRegex = new Regex(@"^/messages/?\d*$");
            MessageApi api = new MessageApi(request, messageRegex, _fileService.Object, "messages");
            _fileService.Setup(s => s.FileExists("messages\\1.txt")).Returns(true);

            string response = api.Interaction();

            Assert.AreEqual("PUT OK", response);
        }
    }
}